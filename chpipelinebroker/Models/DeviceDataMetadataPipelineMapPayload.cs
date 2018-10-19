
public class DeviceDataMetadataPipelineMapPayload
{
    public object Data { get; set; }
    public string DataType { get; set; }
    public int DeviceId { get; set; }
    public PipelineMap Map { get; set; }
    public int PatientId { get; set; }
    public string StudyId { get; set; }
}