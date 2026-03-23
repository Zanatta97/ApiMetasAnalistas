namespace ApiMetasAnalistas.Interfaces
{
    public interface IUnityOfWork
    {
        IAnalystRepository AnalystRepository { get; }
        IHolidayRepository HolidayRepository { get; }
        IOccurrenceRepository OccurrenceRepository { get; }
        IRegionRepository RegionRepository { get; }
        ITicketRepository TicketRepository { get; }
        void Commit();
    }
}
