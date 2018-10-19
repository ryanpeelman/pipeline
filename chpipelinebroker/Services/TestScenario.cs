using System;
using System.Collections.Generic;

public class TestScenario
{
    private static readonly Lazy<TestScenario> _lazyInstance = new Lazy<TestScenario>(() => new TestScenario());

    public static TestScenario Instance
    {
        get
        {
            return _lazyInstance.Value;
        }
    }

    private TestScenario()
    {
        // place for instance initialization code
    }

    public Tuple<Patient, PipelineMap> GetTestScenario()
    {
        var patient = new Patient
        {
            Id = 100987,
            StudyId = "C434100P2"
        };

        var pipelineMap = new PipelineMap
        {
            Routes = new List<PipelineRoute>()
            {
                new PipelineRoute { Path = "log" },
                new PipelineRoute { Path = "normalize" },
                new PipelineRoute
                {
                    Path = "store",
                    Parameters = new
                    {
                        storage = "postgres",
                        connectionString = "Host=ec2-184-72-247-70.compute-1.amazonaws.com;Port=5432;Username=qqopuxrydkqkqi;Password=52f6292962702ea68eebeccfbee158e87f59e9daa77c8b512290c4f1649e00d2;Database=d6ob148gcsl7kb"
                    },
                    BaseUrl = "http://chpipeline.herokuapp.com/api/"
                },
                new PipelineRoute
                {
                    Path = "validate",
                    BaseUrl = "http://chpipeline-eventvalidator.herokuapp.com/"
                }
            }
        };

        return new Tuple<Patient, PipelineMap>(patient, pipelineMap);
    }
}