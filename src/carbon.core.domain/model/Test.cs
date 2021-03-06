using System;
using carbon.core.Features;

namespace carbon.core.domain.model
{
    public class Test : Entity<Guid>
    {
        
        public string Name { get; private set; }
        public int Value { get; private set; }

        protected Test() {}

        public static Test Create()
        {
            var obj = new Test
            {
                Id = Guid.NewGuid(),
                Name = "DEFAULT",
                Value = 100
            };


            return obj;
        }

        public void Update(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}