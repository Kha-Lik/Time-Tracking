using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TimeTracking.Contracts.Events;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Entities;

namespace TimeTracking.Bl.Impl.Consumers
{
    public class UserSignedUpEventConsumer : IConsumer<UserSignedUp>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserSignedUpEventConsumer> _logger;

        public UserSignedUpEventConsumer(IUserRepository userRepository,
            ILogger<UserSignedUpEventConsumer> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<UserSignedUp> context)
        {
            var userExisting = await _userRepository.GetByIdAsync(context.Message.UserId);
            if (userExisting == null)
            {
                try
                {
                    await _userRepository.AddAsync(new TimeTrackingUser()
                    {
                        Id = context.Message.UserId,
                        FirstName = context.Message.FirstName,
                        LastName = context.Message.LastName,
                        Email = context.Message.Email
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An error occurred while adding new user {0}", context.Message);
                    throw;
                }
            }
            _logger.LogWarning("Such user is already exists {0}", context.Message.UserId);

            /*await _userRepository.UpdateAsync(new TimeTrackingUser()
            {
                Id = context.Message.UserId,
                FirstName = context.Message.FirstName,
                LastName = context.Message.LastName,
                Email = context.Message.Email
            });*/
        }
    }
}