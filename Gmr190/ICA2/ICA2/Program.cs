﻿using System;

namespace ICA2
{
	class Program
	{
		static void Main(string[] args)
		{
			double dHighTemperature = 0.0;
			double dLowTemperature = 0.0;
			double dAverage = 0.0;

			try
			{
				Console.WriteLine("Please enter the high temperature: ");
				dHighTemperature = Convert.ToDouble(Console.ReadLine());

				Console.WriteLine("Please enter the low temperature: ");
				dLowTemperature = Convert.ToDouble(Console.ReadLine());

				dAverage = (dHighTemperature + dLowTemperature) / 2.0;

				Console.WriteLine("Average: {0}, High: {1}, Low: {2}", dAverage, dHighTemperature, dLowTemperature);
			}
			catch (Exception e)
			{
				Console.WriteLine("Something went terribly wrong: " + e.Message);
			}
			finally
			{
				Console.ReadLine();
			}
		}
	}
}
