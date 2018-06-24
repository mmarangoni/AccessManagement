using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PrimaryWebApp.Models;

namespace PrimaryWebApp.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private DataContext ds = new DataContext();

        // AutoMapper components
        MapperConfiguration config;
        public IMapper mapper;

        public Manager()
        {
            // Configure AutoMapper...
            config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                cfg.CreateMap<Models.Employee, Controllers.EmployeeBase>();
                cfg.CreateMap<Models.Employee, Controllers.EmployeeWithDetails>();
                cfg.CreateMap<Controllers.EmployeeAdd, Models.Employee>();

            });

            mapper = config.CreateMapper();

            // Data-handling configuration

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // Add methods below
        // Controllers will call these methods
        // Ensure that the methods accept and deliver ONLY view model objects and collections
        // The collection return type is almost always IEnumerable<T>

        // Suggested naming convention: Entity + task/action
        // For example:
        // ProductGetAll()
        // ProductGetById()
        // ProductAdd()
        // ProductEdit()
        // ProductDelete()


        // Method templates, used by the ExampleController class

        public IEnumerable<string> ExampleGetAll()
        {
            return new List<string> { "hello", "world" };
        }

        public string ExampleGetById(int id)
        {
            return $"id {id} was requested";
        }

        public string ExampleAdd(string newItem)
        {
            return $"new item {newItem} was added";
        }

        public string ExampleEditSomething(string editedItem)
        {
            return $"item was edited with {editedItem}";
        }

        public bool ExampleDelete(int id)
        {
            return true;
        }


        // Employee

        public IEnumerable<EmployeeBase> EmployeeGetAll()
        {
            return mapper.Map<IEnumerable<EmployeeBase>>
                (ds.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName));
        }

        public IEnumerable<EmployeeWithDetails> EmployeeGetAllWithOrgInfo()
        {
            // Attention - Manager - Employee - get all, with self-referencing info

            var c = ds.Employees.Include("DirectReports").Include("ReportsTo");

            // Return the result
            return mapper.Map<IEnumerable<EmployeeWithDetails>>
                (c.OrderBy(e => e.LastName).ThenBy(e => e.FirstName));
        }

        public EmployeeWithDetails EmployeeGetByIdWithDetails(int id)
        {
            // Attention - Manager - Employee - get all, with self-referencing info

            // Attempt to get the matching object
            var o = ds.Employees.Include("DirectReports").Include("ReportsTo")
                .SingleOrDefault(e => e.EmployeeId == id);

            // Return the result, or null if not found
            return (o == null) ? null : mapper.Map<EmployeeWithDetails>(o);
        }

        public EmployeeBase EmployeeAdd(EmployeeAdd newItem)
        {
            // Attention - Manager - Employee - add, we are going to allow a "reports to" (manager/supervisor) identifier to be provided
            // Therefore, we must attempt to fetch it
            // We will tolerate success or failure

            // Attempt to find the reports to (manager/supervisor) object
            var a = ds.Employees.Find(newItem.ReportsToId.GetValueOrDefault());

            // Attempt to add the object
            var addedItem = ds.Employees.Add(mapper.Map<Employee>(newItem));
            if (a != null)
            {
                addedItem.ReportsTo = a;
            }
            ds.SaveChanges();

            // Return the result, or null if there was an error
            return (addedItem == null) ? null : mapper.Map<EmployeeBase>(addedItem);
        }

        public EmployeeWithDetails EmployeeSetSupervisor(EmployeeSupervisor newItem)
        {
            // Attention - Manager - Employee - command to update the self-referencing to-one association

            // Attempt to fetch the object
            // Include associated data (look at the return type from this method)
            var o = ds.Employees.Include("DirectReports").Include("ReportsTo")
                .SingleOrDefault(e => e.EmployeeId == newItem.EmployeeId);

            // Attempt to fetch the associated object
            // Include associated data (look at the return type from this method)
            Employee a = null;
            if (newItem.ReportsToId > 0)
            {
                a = ds.Employees.Include("DirectReports").Include("ReportsTo")
                    .SingleOrDefault(e => e.EmployeeId == newItem.ReportsToId);
            }

            // Must do two tests here before continuing
            if (o == null | a == null)
            {
                // Problem - one of the items was not found, so return
                return null;
            }
            else
            {
                // Configure the new supervisor
                // MUST set both properties - the int and the Employee
                o.ReportsTo = a;
                o.ReportsToId = a.EmployeeId;

                ds.SaveChanges();

                // Prepare and return the object
                return mapper.Map<EmployeeWithDetails>(o);
            }
        }

        public EmployeeWithDetails EmployeeSetDirectReports(EmployeeDirectReports newItem)
        {
            // Attention - Manager - Employee - command to update the self-referencing to-many association

            // Attempt to fetch the object

            // When editing an object with a to-many collection,
            // and you wish to edit the collection,
            // MUST fetch its associated collection
            var o = ds.Employees.Include("DirectReports").Include("ReportsTo")
                .SingleOrDefault(e => e.EmployeeId == newItem.EmployeeId);

            if (o == null)
            {
                // Problem - object was not found, so return
                return null;
            }
            else
            {
                // Update the object with the incoming values

                // First, clear out the existing collection
                // "DirectReports" is the badly-named to-many collection property
                o.DirectReports.Clear();

                // Then, go through the incoming items
                // For each one, add to the fetched object's collection
                foreach (var item in newItem.EmployeeIds)
                {
                    var a = ds.Employees.Find(item);
                    if (a != null)
                    {
                        o.DirectReports.Add(a);
                    }
                }
                // Save changes
                ds.SaveChanges();

                return mapper.Map<EmployeeWithDetails>(o);
            }
        }


        // Programmatically-generated objects

        // Can do this in one method, or in several
        // Call the method(s) from a controller method

        public bool LoadData()
        {
            /*
            // Return immediately if there's existing data
            if (ds.[entity collection].Courses.Count() > 0) { return false; }

            // Otherwise, add objects...

            ds.[entity collection].Add(new [whatever] { Property1 = "value", Property2 = "value" });
            */

            return ds.SaveChanges() > 0 ? true : false;
        }

    }
}