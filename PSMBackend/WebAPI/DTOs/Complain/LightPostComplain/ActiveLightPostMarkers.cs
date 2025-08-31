using System;
using PSMModel.Models;

namespace PSMWebAPI.DTOs.Complain.LightPostComplain;

public class ActiveLightPostMarkers
{
    public string LightPostNumber { get; set; }
    public LightPost LightPost { get; set; }
    public List<PSMModel.Models.LightPostComplain> Complains { get; set; }
}
