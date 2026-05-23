using VoguMap.Domain.Entities;
using VoguMap.Infrastructure.Persistence.Context;

namespace VoguMap.Infrastructure.Persistence.Seed
{
    public static class ApplicationDbSeeder
    {
        public static void SeedBuildings(VoguMapContext context)
        {
            if (!context.Buildings.Any())
            {
                context.Buildings.AddRange(
                    new Building
                    {
                        Id = 1,
                        Name = "Учебный корпус №1",
                        Address = "Галкинская улица, 3, Вологда, 160000",
                        Latitude = 59.219346m,
                        Longitude = 39.898141m
                    },
                    new Building
                    {
                        Id = 2,
                        Name = "Учебный корпус №2",
                        Address = "Галкинская улица, 1, Вологда, 160000",
                        Latitude = 59.220201m,
                        Longitude = 39.898939m
                    },
                    new Building
                    {
                        Id = 3,
                        Name = "Учебный корпус №3",
                        Address = "улица Гагарина, 81А, Вологда, 160002",
                        Latitude = 59.205379m,
                        Longitude = 39.831019m
                    },
                    new Building
                    {
                        Id = 4,
                        Name = "Учебный корпус №4",
                        Address = "Предтеченская улица, 20, Вологда, 160000",
                        Latitude = 59.218410m,
                        Longitude = 39.899329m
                    },
                    new Building
                    {
                        Id = 5,
                        Name = "Учебный корпус №5",
                        Address = "улица Ленина, 15, Вологда, 160000",
                        Latitude = 59.222550m,
                        Longitude = 39.897164m
                    },
                    new Building
                    {
                        Id = 6,
                        Name = "Учебный корпус №6",
                        Address = "проспект Победы, 2, Вологда",
                        Latitude = 59.221538m,
                        Longitude = 39.889012m
                    },
                    new Building
                    {
                        Id = 7,
                        Name = "Учебный корпус №7",
                        Address = "улица Сергея Орлова, 6, Вологда, 160000",
                        Latitude = 59.223904m,
                        Longitude = 39.887453m
                    },
                    new Building
                    {
                        Id = 8,
                        Name = "Учебный корпус №8",
                        Address = "проспект Победы, 37, Вологда, 160001",
                        Latitude = 59.222808m,
                        Longitude = 39.878919m
                    },
                    new Building
                    {
                        Id = 9,
                        Name = "Учебный корпус №9",
                        Address = "улица Мальцева, 2, Вологда",
                        Latitude = 59.222431m,
                        Longitude = 39.879950m
                    },
                    new Building
                    {
                        Id = 10,
                        Name = "Учебный корпус №10",
                        Address = "улица Мальцева, 22, Вологда, 160001",
                        Latitude = 59.220174m,
                        Longitude = 39.878066m
                    },
                    new Building
                    {
                        Id = 11,
                        Name = "Учебный корпус №11",
                        Address = "проспект Победы, 71, Вологда, 160000",
                        Latitude = 59.224651m,
                        Longitude = 39.870710m
                    },
                    new Building
                    {
                        Id = 12,
                        Name = "Учебный корпус №12‑13",
                        Address = "улица Ильюшина, 15, Вологда, 160028",
                        Latitude = 59.209426m,
                        Longitude = 39.819012m
                    }
                );

                context.SaveChanges();
            }
        }
    }
}