using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;

namespace ApiMetasAnalistas.Repositories
{
    public class UnityOfWork : IUnityOfWork
    {
        private IAnalystRepository? _analystRepository;
        private IHolidayRepository? _holidayRepository;
        private IOccurrenceRepository? _occurrenceRepository;
        private IRegionRepository? _regionRepository;
        private ITicketRepository? _ticketRepository;

        public AppDBContext _context;

        public UnityOfWork(AppDBContext context)
        { 
            _context = context;
        }

        public IAnalystRepository AnalystRepository
        {
            get
            {
                return _analystRepository = _analystRepository ?? new AnalystRepository(_context);
            }
        }

        public IHolidayRepository HolidayRepository
        {
            get
            {
                return _holidayRepository = _holidayRepository ?? new HolidayRepository(_context);
            }
        }

        public IOccurrenceRepository OccurrenceRepository
        {
            get
            {
                return _occurrenceRepository = _occurrenceRepository ?? new OccurrenceRepository(_context);
            }
        }

        public IRegionRepository RegionRepository
        {
            get
            {
                return _regionRepository = _regionRepository ?? new RegionRepository(_context);
            }
        }

        public ITicketRepository TicketRepository
        {
            get
            {
                return _ticketRepository = _ticketRepository ?? new TicketRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
