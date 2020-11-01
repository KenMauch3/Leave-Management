﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Leave_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Management.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationsController : Controller
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationsController(
            ILeaveTypeRepository leaveTypeRepository, 
            ILeaveAllocationRepository leaveAllocationRepository, 
            IMapper mapper,
            UserManager<Employee> userManager)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: LeaveAllocationController
        public async Task<ActionResult> Index(int numberUpdated)
        {
            var leaveTypes = await _leaveTypeRepository.FindAll();
            var mappedLeaveTypes = _mapper.Map<List<Data.LeaveType>, List<LeaveTypeViewModel>>(leaveTypes.ToList());
            var model = new CreateLeaveAllocationViewModel { LeaveTypes = mappedLeaveTypes, NumberUpdated = numberUpdated };
            return View(model);
        }

        public async Task<ActionResult> SetLeave(int id)
        {
            var leaveType = await _leaveTypeRepository.FindById(id);
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            int allocationCount = 0;
            foreach (var employee in employees)
            {
                if (await _leaveAllocationRepository.CheckAllocation(id, employee.Id))
                    continue;

                var allocation = new LeaveAllocationViewModel
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = employee.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = DateTime.Now.Year
                };
                var leaveAllocation = _mapper.Map<LeaveAllocation>(allocation);
                await _leaveAllocationRepository.Create(leaveAllocation);
                allocationCount++;
            }
            return RedirectToAction(nameof(Index), new { numberUpdated = allocationCount });
        }

        public async Task<ActionResult> ListEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            var model = _mapper.Map<List<EmployeeViewModel>>(employees);
            return View(model);
        }

        // GET: LeaveAllocationController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var employee = _mapper.Map<EmployeeViewModel>(await _userManager.FindByIdAsync(id));
            var allocations = _mapper.Map<List<LeaveAllocationViewModel>>(await _leaveAllocationRepository.GetLeaveAllocationsByEmployee(id));
            var model = new ViewLeaveAllocationsViewModel
            {
                Employee = employee,
                EmployeeId = id,
                LeaveAllocations = allocations
            };
            return View(model);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var allocation = await _leaveAllocationRepository.FindById(id);
            var model = _mapper.Map<EditLeaveAllocationViewModel>(allocation);

            return View(model);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditLeaveAllocationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = await _leaveAllocationRepository.FindById(model.Id);
                record.NumberOfDays = model.NumberOfDays;
                var isSuccess = await _leaveAllocationRepository.Update(record);
                if(!isSuccess)
                {
                    ModelState.AddModelError("", "Error while saving");
                    return View(model);
                }
                return RedirectToAction(nameof(Details), new { id = model.EmployeeId });
            }
            catch
            {
                return View(model);
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
