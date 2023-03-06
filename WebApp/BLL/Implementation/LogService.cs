using AutoMapper;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LogService : ILogService
    {
        readonly IDataProvider _dataProvider;
        readonly IMapper _mapper;

        public LogService(IDataProvider dataProvider, IMapper mapper)
        {
            _dataProvider = dataProvider;
            _mapper = mapper;
        }

        async public Task AddLog(LogDto dto)
        {
            var dao = _mapper.Map<Log>(dto);
            await _dataProvider.Insert<Log>(dao);
            await _dataProvider.SaveAsync();
        }
    }
}
