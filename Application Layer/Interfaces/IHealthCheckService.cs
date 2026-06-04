using Domain_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Layer.DTOs;

namespace Application_Layer.Interfaces;

public interface IHealthCheckService
{
    Task<HealthStatus> GetStatusAsync();
    void RecordSuccess(string component);
    void RecordFailure(string component, string error);
}