using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class LightPostComplain : Complain
{
    [ForeignKey("LightPost")]
    public string LightPostNumber { get; set; }

    public LightPost LightPost { get; set; }
}
