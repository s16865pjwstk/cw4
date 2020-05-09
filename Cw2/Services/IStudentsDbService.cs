using Cw2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw2.Services
{
    public interface IStudentsDbService
    {
        public EnrollmentResponse enrollment(EnrollmentRequest enrollment);
    }
}
