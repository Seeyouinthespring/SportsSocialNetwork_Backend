using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class UserService : IUserService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ICommonRepository commonRepository, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _commonRepository = commonRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> GetRoleByNameAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.First();
            //string role = _roleManager.GetRoleNameAsync();

        }

        public ApplicationUser GetUserByNameAsync(string name)
        {
            var entity = _commonRepository.FindByCondition<ApplicationUser>(x => x.NormalizedUserName == name.ToUpper())
                .Include(x => x.ContactInformation)
                .FirstOrDefault();

            if (entity == null)
                return null;
            return entity;
        }

        public async Task UpdatePhotoAsync(PhotoModel model,string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.Photo = Convert.FromBase64String(model.Photo);

            _commonRepository.Update(user);
            await _commonRepository.SaveAsync();
        }

        public async Task<ProfileViewModel> GetProfileAsync(string userId, DateTime currentDate) 
        {
            ApplicationUser entity = await _commonRepository.FindByCondition<ApplicationUser>(x => x.Id == userId)
                .Select(x => new ApplicationUser {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Photo = x.Photo,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    ContactInformation = new ContactInformation 
                    {
                        Vk = x.ContactInformation.Vk,
                        PhoneNumber = x.ContactInformation.PhoneNumber,
                        Email = x.ContactInformation.Email,
                        Instagram = x.ContactInformation.Instagram
                    },
                    VisitedAppointments = x.VisitedAppointments.Select(a => new AppointmentVisiting {
                        Status = a.Status,
                        Appointment = new Appointment 
                        {
                            Id = a.Appointment.Id,
                            Date = a.Appointment.Date,
                            StartTime = a.Appointment.StartTime,
                            EndTime = a.Appointment.EndTime,
                            PlaygroundId = a.Appointment.PlaygroundId,
                            Playground = new Playground 
                            {
                                Name = a.Appointment.Playground.Name
                            }
                        }
                    }).ToList(),
                    ConfirmedRents = x.ConfirmedRents.Select(r => new ConfirmedRent 
                    {
                        Id = r.Id,
                        Date = r.Date,
                        StartTime = r.StartTime,
                        EndTime = r.EndTime,
                        IsExecuted = r.IsExecuted,
                        PlaygroundId = r.PlaygroundId,
                        Playground = new Playground 
                        {
                            Name = r.Playground.Name
                        }
                    }).ToList(),
                    PersonalActivities = x.PersonalActivities.Select(p => new PersonalActivity 
                    { 
                        Id = p.Id,
                        Date = p.Date,
                        StartTime = p.StartTime,
                        EndTime = p.EndTime,
                        IsVisited = p.IsVisited,
                        PlaygroundId = p.PlaygroundId,
                        Playground = new Playground 
                        {
                            Name = p.Playground.Name
                        }
                    }).ToList()

                })
                //.Include(x => x.ContactInformation)
                //.Include(x => x.VisitedAppointments).ThenInclude(x => x.Appointment).ThenInclude(x => x.Playground)
                //.Include(x => x.PersonalActivities).ThenInclude(x => x.Playground)
                //.Include(x => x.ConfirmedRents).ThenInclude(x => x.Playground)
                .FirstOrDefaultAsync();

            List<CommonActivityViewModel> latestActivities = new List<CommonActivityViewModel>();
            latestActivities.AddRange(
                entity.VisitedAppointments
                .Where(x => x.Appointment.Date < currentDate && x.Appointment.Date > currentDate.AddDays(-30))
                .ToList()
                .MapTo<List<CommonActivityViewModel>>());
            latestActivities.AddRange(
                entity.PersonalActivities
                .Where(x => x.Date < currentDate && x.Date > currentDate.AddDays(-30))
                .ToList()
                .MapTo<List<CommonActivityViewModel>>());
            latestActivities.AddRange(
                entity.ConfirmedRents
                .Where(x => x.Date < currentDate && x.Date > currentDate.AddDays(-30))
                .ToList()
                .MapTo<List<CommonActivityViewModel>>());

            List<CommonActivityViewModel> nearestActivities = new List<CommonActivityViewModel>();
            nearestActivities.AddRange(
                entity.VisitedAppointments
                .Where(x =>x.Appointment.Date > currentDate && x.Appointment.Date < currentDate.AddDays(30))
                .ToList()
                .MapTo<List<CommonActivityViewModel>>());
            nearestActivities.AddRange(
                entity.PersonalActivities
                .Where(x => x.Date > currentDate && x.Date < currentDate.AddDays(30))
                .ToList()
                .MapTo<List<CommonActivityViewModel>>());
            nearestActivities.AddRange(
                entity.ConfirmedRents
                .Where(x => x.Date > currentDate && x.Date < currentDate.AddDays(30))
                .ToList()
                .MapTo<List<CommonActivityViewModel>>());

            ProfileViewModel result = entity.MapTo<ProfileViewModel>();
            result.NearestActivities = nearestActivities.OrderByDescending(x => x.Date).ToList();
            result.LatestActivities = latestActivities.OrderByDescending(x => x.Date).ToList();
            return result;
        }

        public async Task UpdatePhotoAsync(byte[] bytes, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.Photo = bytes;

            _commonRepository.Update(user);
            await _commonRepository.SaveAsync();
        }
    }
}
