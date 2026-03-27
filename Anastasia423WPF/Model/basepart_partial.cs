using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anastasia423WPF;
namespace Anastasia423WPF
{
    public partial class basepart_
    {
        public string DisplayDetails
        {
            get
            {

                if (this.cpu_ != null)
                {
                    return $"Ядра: {this.cpu_.numberofcores} шт. | Базовая частота: {this.cpu_.basecorefrequency} ГГц | TDP: {this.cpu_.thermalpower} Вт";
                }

                if (this.gpu_ != null)
                {
                    return $"Память: {this.gpu_.videomemory} ГБ | Шина: {this.gpu_.memorybus} бит | Реком. БП: {this.gpu_.recommendpower} Вт";
                }

                if (this.ram_ != null)
                {
                    return $"Объем: {this.ram_.capacity} ГБ | Частота: {this.ram_.ghz} МГц | Тайминги: {this.ram_.timings}";
                }

                if (this.powersupply_ != null)
                {
                    return $"Мощность: {this.powersupply_.power} Вт";
                }

                if (this.storagedevice_ != null)
                {
                    return $"Объем: {this.storagedevice_.capacity} ГБ";
                }

                if (this.processorcooler_ != null)
                {
                    return $"Уровень шума: {this.processorcooler_.noiselevel} дБ | Обороты: {this.processorcooler_.minspeed}-{this.processorcooler_.maxspeed} об/мин";
                }

                return string.Empty; // Если это корпус или плата без доп. деталей
            }
        }
    }
}
