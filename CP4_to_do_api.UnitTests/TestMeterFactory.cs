using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace CP4_to_do_api.UnitTests
{
    /// <summary>
    /// Fake simples de IMeterFactory para os testes de unidade do TarefaService.
    /// Como nao ha nada a verificar sobre ele (nao faz parte do comportamento
    /// testado), e mais simples usar essa implementacao minima real do que
    /// configurar um Mock&lt;IMeterFactory&gt;.
    /// </summary>
    internal sealed class TestMeterFactory : IMeterFactory
    {
        private readonly List<Meter> _meters = new();

        public Meter Create(MeterOptions options)
        {
            var meter = new Meter(options.Name, options.Version);
            _meters.Add(meter);
            return meter;
        }

        public void Dispose()
        {
            foreach (var meter in _meters)
            {
                meter.Dispose();
            }
            _meters.Clear();
        }
    }
}
