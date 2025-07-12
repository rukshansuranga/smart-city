using System;
using System.Drawing;
using PSMDataAccess;
using PSMModel.Models;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Services;

public class RideSimulationService : BackgroundService
{
    private readonly ApplicationDbContext _context;
    private List<Point> _ridePoints = new List<Point>();
    public RideSimulationService(IServiceProvider serviceProvider)
    {
        //_context = context;
        _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _ridePoints.Add(new Point {Latitude=7.024987, Longitude=80.026454});
        _ridePoints.Add(new Point {Latitude=7.024982, Longitude=80.026364});
        _ridePoints.Add(new Point {Latitude=7.025004, Longitude=80.026232}); 
        _ridePoints.Add(new Point {Latitude=7.025021, Longitude=80.026078});
        _ridePoints.Add(new Point {Latitude=7.025042, Longitude=80.025892});
        _ridePoints.Add(new Point {Latitude=7.025074, Longitude=80.025669}); 
        _ridePoints.Add(new Point {Latitude=7.025119, Longitude=80.025407}); 
        _ridePoints.Add(new Point {Latitude=7.025221, Longitude=80.025130}); 
        _ridePoints.Add(new Point {Latitude=7.025353, Longitude=80.024694}); 
        _ridePoints.Add(new Point {Latitude=7.025424, Longitude=80.024085}); 
        _ridePoints.Add(new Point {Latitude=7.025469, Longitude=80.023584}); 
        _ridePoints.Add(new Point {Latitude=7.025447, Longitude=80.023038}); 
        _ridePoints.Add(new Point {Latitude=7.025165, Longitude=80.022555}); 
        _ridePoints.Add(new Point {Latitude=7.024663, Longitude=80.022583}); 
        _ridePoints.Add(new Point {Latitude=7.024268, Longitude=80.022530}); 
        _ridePoints.Add(new Point {Latitude=7.024009, Longitude=80.022238}); 
        _ridePoints.Add(new Point {Latitude=7.023695, Longitude=80.021804}); 
        _ridePoints.Add(new Point {Latitude=7.023386, Longitude=80.021348}); 
        _ridePoints.Add(new Point {Latitude=7.023029, Longitude=80.021289}); 
        _ridePoints.Add(new Point {Latitude=7.022694, Longitude=80.021487}); 
        _ridePoints.Add(new Point {Latitude=7.022563, Longitude=80.021677}); 
        _ridePoints.Add(new Point {Latitude=7.022431, Longitude=80.021861}); 
        _ridePoints.Add(new Point {Latitude=7.022403, Longitude=80.022144}); 
        _ridePoints.Add(new Point {Latitude=7.022501, Longitude=80.022383}); 
        _ridePoints.Add(new Point {Latitude=7.022606, Longitude=80.022597});
        _ridePoints.Add(new Point {Latitude=7.022700, Longitude=80.022750}); 
        _ridePoints.Add(new Point {Latitude=7.022780, Longitude=80.022893}); 
        _ridePoints.Add(new Point {Latitude=7.022859, Longitude=80.023179}); 
        _ridePoints.Add(new Point {Latitude=7.022874, Longitude=80.023418}); 
        _ridePoints.Add(new Point {Latitude=7.022815, Longitude=80.023659}); 
        _ridePoints.Add(new Point {Latitude=7.022742, Longitude=80.023856}); 
        _ridePoints.Add(new Point {Latitude=7.022668, Longitude=80.024053}); 
        _ridePoints.Add(new Point {Latitude=7.022559, Longitude=80.024351});
        _ridePoints.Add(new Point {Latitude=7.022454, Longitude=80.024628}); 
        _ridePoints.Add(new Point {Latitude=7.022384, Longitude=80.024808}); 
        _ridePoints.Add(new Point {Latitude=7.022307, Longitude=80.025040});
        _ridePoints.Add(new Point {Latitude=7.022168, Longitude=80.025212});
        _ridePoints.Add(new Point {Latitude=7.022051, Longitude=80.025332});
        _ridePoints.Add(new Point {Latitude=7.021853, Longitude=80.025537}); 
        _ridePoints.Add(new Point {Latitude=7.021686, Longitude=80.025736}); 
        _ridePoints.Add(new Point {Latitude=7.021528, Longitude=80.025920}); 
        _ridePoints.Add(new Point {Latitude=7.021336, Longitude=80.026176}); 
        _ridePoints.Add(new Point {Latitude=7.021402, Longitude=80.026508}); 
        _ridePoints.Add(new Point {Latitude=7.021535, Longitude=80.026517}); 
        _ridePoints.Add(new Point {Latitude=7.021848, Longitude=80.026602});
        _ridePoints.Add(new Point {Latitude=7.022219, Longitude=80.026684});
        _ridePoints.Add(new Point {Latitude=7.022469, Longitude=80.026754});
        _ridePoints.Add(new Point {Latitude=7.022876, Longitude=80.026783});
        _ridePoints.Add(new Point {Latitude=7.023201, Longitude=80.026767});
        _ridePoints.Add(new Point {Latitude=7.023591, Longitude=80.026661});
        _ridePoints.Add(new Point {Latitude=7.023886, Longitude=80.026561});
        _ridePoints.Add(new Point {Latitude=7.024189, Longitude=80.026538});
        _ridePoints.Add(new Point {Latitude=7.024532, Longitude=80.026553});
        _ridePoints.Add(new Point {Latitude=7.024651, Longitude=80.026544});
        _ridePoints.Add(new Point {Latitude=7.024886, Longitude=80.026582});
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //Your background work logic here
            foreach (Point point in _ridePoints)
            {
                _context.RidePoints.Add(new RidePoint { RideId = 1, Latitude = point.Latitude, Longitude = point.Longitude, PointTime = PSMDateTime.Now });
                await _context.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

        }
    }

}

public class Point {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}