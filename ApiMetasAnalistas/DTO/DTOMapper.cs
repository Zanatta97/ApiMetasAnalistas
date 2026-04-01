using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.DTO
{
    public static class DTOMapper
    {
        public static Analyst? ToEntity(this AnalystRequestDTO dto)
        {
            if (dto is null)
                return null;

            return new Analyst(
                dto.Nome,
                dto.Usuario,
                dto.RegiaoId,
                dto.MetaDiaria
            );
        }

        public static AnalystResponseDTO? ToDTO(this Analyst analyst)
        {
            if (analyst is null)
                return null;

            return new AnalystResponseDTO
            {
                Id = analyst.Id,
                Nome = analyst.Nome,
                Usuario = analyst.Usuario,
                RegiaoId = analyst.RegiaoId,
                MetaDiaria = analyst.MetaDiaria
            };
        }

        public static IEnumerable<AnalystResponseDTO> ToDTOList(this IEnumerable<Analyst> analysts)
        {
            if (analysts is null || !analysts.Any())
                return new List<AnalystResponseDTO>();

            return analysts.Select(a => a.ToDTO()!).ToList();
        }

        public static Holiday? ToEntity(this HolidayRequestDTO dto)
        {
            if (dto is null) 
               return null;

            return new Holiday
            (
                dto.Data,
                dto.Descricao,
                dto.RegiaoId
            );
        }

        public static HolidayResponseDTO? ToDTO(this Holiday holiday)
        {
            if (holiday is null)
                return null;

            return new HolidayResponseDTO
            {
                Id = holiday.Id,
                Data = holiday.Data,
                Descricao = holiday.Descricao,
                RegiaoId = holiday.RegiaoId
            };
        }

        public static IEnumerable<HolidayResponseDTO> ToDTOList(this IEnumerable<Holiday> holidays)
        {
            if (holidays is null || !holidays.Any())
                return new List<HolidayResponseDTO>();

            return holidays.Select(h => h.ToDTO()!).ToList();
        }

        public static Occurrence? ToEntity(this OccurrenceRequestDTO dto)
        {
            if (dto is null)
                return null;

            return new Occurrence
            (
                dto.Tipo,
                dto.Descricao,
                dto.AnalistaId,
                dto.DataInicio,
                dto.DataFim
            );
        }

        public static OccurrenceResponseDTO? ToDTO(this Occurrence occurrence)
        {
            if (occurrence is null)
                return null;

            return new OccurrenceResponseDTO
            {
                Id = occurrence.Id,
                Tipo = occurrence.Tipo,
                AnalistaId = occurrence.AnalistaId,
                Descricao = occurrence.Descricao
            };
        }

        public static IEnumerable<OccurrenceResponseDTO> ToDTOList(this IEnumerable<Occurrence> occurrences)
        {
            if (occurrences is null || !occurrences.Any())
                return new List<OccurrenceResponseDTO>();

            return occurrences.Select(o => o.ToDTO()!).ToList();
        }

        public static Region? ToEntity(this RegionRequestDTO dto)
        {
            if (dto is null)
                return null;

            return new Region
            (
                dto.Nome
            );
        }

        public static RegionResponseDTO? ToDTO(this Region region)
        {
            if (region is null)
                return null;

            return new RegionResponseDTO
            {
                Id = region.Id,
                Nome = region.Nome
            };
        }

        public static IEnumerable<RegionResponseDTO> ToDTOList(this IEnumerable<Region> regions)
        {
            if (regions is null || !regions.Any())
                return new List<RegionResponseDTO>();

            return regions.Select(r => r.ToDTO()!).ToList();
        }

        public static Ticket? ToEntity(this TicketRequestDTO dto)
        {
            if (dto is null) 
               return null;

            return new Ticket
            (
                dto.AnalystId,
                dto.DataFechamento
            );
        }

        public static TicketResponseDTO? ToDTO(this Ticket ticket)
        {
            if (ticket is null) 
               return null;

            return new TicketResponseDTO
            {
                Id = ticket.Id,
                DataFechamento = ticket.DataFechamento,
                AnalystId = ticket.AnalystId
            };
        }

        public static IEnumerable<TicketResponseDTO> ToDTOList(this IEnumerable<Ticket> tickets)
        {
            if (tickets is null || !tickets.Any())
                return new List<TicketResponseDTO>();

            return tickets.Select(t => t.ToDTO()!).ToList();
        }
    }
}
