using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
    public override string ToString() => Newtonsoft.Json.JsonConvert.SerializeObject(this);
}

