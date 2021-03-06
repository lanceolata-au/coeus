using System;

namespace carbon.core.dtos.model.registration
{
    public class PreliminaryApplicationDto
    {
        public int EventId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public StatusEnum Status { get; set; }
        public string DateOfBirth { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
    }
}