﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using CramerAlexa.Services;

namespace CramerAlexa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var rooms = _roomService.GetRooms();
            return Json(new { rooms = rooms });
        }

        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
