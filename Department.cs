using System;

namespace RIS_p2_LR3
{
    /// <summary>
    /// Represents a department with employee count information
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EmployeeCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Department()
        {
            CreatedDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
        }

        public Department(int id, string name, string description, int employeeCount)
        {
            Id = id;
            Name = name;
            Description = description;
            EmployeeCount = employeeCount;
            CreatedDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Название: {Name}, Описание: {Description}, Количество сотрудников: {EmployeeCount}, " +
                   $"Создано: {CreatedDate:dd.MM.yyyy}, Изменено: {LastModifiedDate:dd.MM.yyyy}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Department other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
