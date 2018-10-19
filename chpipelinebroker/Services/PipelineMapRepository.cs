using System;
using System.Collections.Generic;

public class PipelineMapRepository
{
    private static readonly Lazy<PipelineMapRepository> _lazyInstance = new Lazy<PipelineMapRepository>(() => new PipelineMapRepository());

    public static PipelineMapRepository Instance
    {
        get
        {
            return _lazyInstance.Value;
        }
    }

    private PipelineMapRepository()
    {
        // place for instance initialization code
    }

    public PipelineMap GetPipelineMapByPatientId(int patientId)
    {
        return TestScenario.Instance.GetTestScenario().Item2;
    }
}