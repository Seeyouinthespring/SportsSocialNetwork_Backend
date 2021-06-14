using SportsSocialNetwork.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Helpers
{
    public static class RentQueryHelper
    {
        public static IQueryable<RentRequest> SelectData(this IQueryable<RentRequest> query, bool check) 
        {
            if (check)
                return query.Select(x => new RentRequest
                {
                    Date = x.Date,
                    Id = x.Id,
                    Description = x.Description,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    IsOnce = x.IsOnce,
                    DayOfTheWeek = x.DayOfTheWeek,
                    Playground = new Playground
                    {
                        Latitude = x.Playground.Latitude,
                        Longitude = x.Playground.Longitude,
                        Name = x.Playground.Name,
                        Photo = x.Playground.Photo,
                        Street = x.Playground.Street,
                        City = x.Playground.City,
                        HouseNumber = x.Playground.HouseNumber,
                        Id = x.Id,
                        ResponsiblePerson = new ApplicationUser
                        {
                            Id = x.Playground.ResponsiblePerson.Id,
                            Photo = x.Playground.ResponsiblePerson.Photo,
                            LastName = x.Playground.ResponsiblePerson.LastName,
                            FirstName = x.Playground.ResponsiblePerson.FirstName,
                            Gender = x.Playground.ResponsiblePerson.Gender,
                            DateOfBirth = x.Playground.ResponsiblePerson.DateOfBirth,
                        }
                    }
                });
            else
                return query.Select(x => new RentRequest
                {
                    Date = x.Date,
                    Id = x.Id,
                    Description = x.Description,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    IsOnce = x.IsOnce,
                    DayOfTheWeek = x.DayOfTheWeek,
                    Renter = new ApplicationUser 
                    {
                        Id = x.Renter.Id,
                        Photo = x.Renter.Photo,
                        LastName = x.Renter.LastName,
                        FirstName = x.Renter.FirstName,
                        Gender = x.Renter.Gender,
                        DateOfBirth = x.Renter.DateOfBirth,
                    },
                    Playground = new Playground
                    {
                        Latitude = x.Playground.Latitude,
                        Longitude = x.Playground.Longitude,
                        Name = x.Playground.Name,
                        Photo = x.Playground.Photo,
                        Street = x.Playground.Street,
                        City = x.Playground.City,
                        HouseNumber = x.Playground.HouseNumber,
                        Id = x.Id
                    }
                });
        }
    }
}
