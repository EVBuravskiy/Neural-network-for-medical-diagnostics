namespace VisualInterface
{
    internal class Patient
    {
        public double Age { get; set; }
        public double Sex { get; set; }
        public double ChestPainType { get; set; }
        public double BloodPressure { get; set; }
        public double Cholestoral { get; set; }
        public double Sugar { get; set; }
        public double Electrocardiographic { get; set; }
        public double HeartRate { get; set; }
        public double InducedAngina { get; set; }
        public double StDepresion { get; set; }
        public double Slope { get; set; }
        public double NumberOfMajorVessels { get; set; }
        public double Thal { get; set; }

        public Patient() { }

        public Patient(
            int age, int sex, int chestPainType, int bloodPressure, 
            int cholestoral, int sugar, int electrocardiographic, int heartRate,
            int inducedAngina, double stDepresion, int slope, int numberOfMajorVessels,
            int thal)
        {
            Age = age;
            Sex = sex;
            ChestPainType = chestPainType;
            BloodPressure = bloodPressure;
            Cholestoral = cholestoral;
            Sugar = sugar;
            Electrocardiographic = electrocardiographic;
            HeartRate = heartRate;
            InducedAngina = inducedAngina;
            StDepresion = stDepresion;
            Slope = slope;
            NumberOfMajorVessels = numberOfMajorVessels;
            Thal = thal;
        }
    }
}
