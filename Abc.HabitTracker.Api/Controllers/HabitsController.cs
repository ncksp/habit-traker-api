using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Abc.HabitTracker.Api.Tracker.Habit;
using Abc.HabitTracker.Api.Tracker;
using Abc.HabitTracker.Api.Tracker.Response;

namespace Abc.HabitTracker.Api.Controllers
{
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly ILogger<HabitsController> _logger;

        public HabitsController(ILogger<HabitsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("api/v1/users/{userID}/habits")]
        public ActionResult<List<HabitResponse>> All(Guid userID)
        {

            var data = Habit.GetHabits(userID);

            if(data == null)
            {
                return NotFound("user not found");
            }

            return data;
        }

        [HttpGet("api/v1/users/{userID}/habits/{id}")]
        public ActionResult<HabitResponse> Get(Guid userID, Guid id)
        {
            var data = Habit.GetHabit(userID, id);

            if (data == null)
            {
                return NotFound("habit not found");
            }

            return data;
        }

        [HttpPost("api/v1/users/{userID}/habits")]
        public ActionResult<Habit> AddNewHabit(Guid userID, [FromBody] RequestData data)
        {
            var result =  Habit.NewHabit(data, userID); ;
            if(result == null)
                return NotFound("User not found");

            return result;
        }

        [HttpPut("api/v1/users/{userID}/habits/{id}")]
        public ActionResult<HabitResponse> UpdateHabit(Guid userID, Guid id, [FromBody] RequestData data)
        {
            var result = Habit.UpdateHabit(id, data, userID); ;
            if (result == null)
                return NotFound("User not found");

            return result;
        }

        [HttpDelete("api/v1/users/{userID}/habits/{id}")]
        public ActionResult<HabitResponse> DeleteHabit(Guid userID, Guid id)
        {
            var result = Habit.DeleteHabit(id, userID);
            if(result == null)
                return NotFound("Habit not found");

            return result;
        }

        [HttpPost("api/v1/users/{userID}/habits/{id}/logs")]
        public ActionResult<HabitResponse> Log(Guid userID, Guid id)
        {
            var result =  Habit.AddLog(userID, id);
            if (result == null)
                return NotFound("Habit not found");

            return result;
        }


    }
}
