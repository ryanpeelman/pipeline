using System;

public class PatientRepository
{
    private static readonly Lazy<PatientRepository> _lazyInstance = new Lazy<PatientRepository>(() => new PatientRepository());

    public static PatientRepository Instance
    {
        get
        {
            return _lazyInstance.Value;
        }
    }

    private PatientRepository()
    {
        // place for instance initialization code
    }

    public Patient GetPatientByDeviceId(int deviceId)
    {
        return TestScenario.Instance.GetTestScenario().Item1;
    }
}