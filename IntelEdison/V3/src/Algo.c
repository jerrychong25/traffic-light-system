/*
 * Algo.c
 *
 *  Created on: Jan 22, 2016
 *      Author: user
 */

#include <stdio.h>
#include <stdlib.h>

float ratio[4];
char ratio_priority[] = {1,2,3,4};

int main()
{
  /* Setup your example here, code that should run once
   */
	int junc1, junc2, junc3, junc4;
	int volume1, volume2, volume3, volume4;
	float ratio1, ratio2, ratio3, ratio4;
	int effective1, effective2, effective3, effective4;
	int green1, green2, green3, green4;
	int res1, res2, res3;
	int cycle = 300;						// One Complete Cycle = 300 Seconds
	int max_flow_rate = 50;

  /* Code in this loop will run repeatedly
   */
	while(1)
	{
		junc1 = count_vehicle;
		junc2 = count_vehicle;
		junc3 = count_vehicle;
		junc4 = count_vehicle;

		volume1 = current_traffic_volume(junc1, cycle);
		volume2 = current_traffic_volume(junc2, cycle);
		volume3 = current_traffic_volume(junc3, cycle);
		volume4 = current_traffic_volume(junc4, cycle);

		ratio1 = flow_ratio(volume1, max_flow_rate);
		ratio2 = flow_ratio(volume2, max_flow_rate);
		ratio3 = flow_ratio(volume3, max_flow_rate);
		ratio4 = flow_ratio(volume4, max_flow_rate);

		ratio[0] = compare(ratio1, ratio2, ratio3, ratio4);		// Max Ratio

		effective1 = effective_green_time(ratio[0], cycle);

		green1 = phase_green_time(effective1);

		res1 = remaining_green_time(effective1, green1);

		effective2 = remaining_effective_green_time(res1, ratio[1], ratio[0], 1);

		green2 = phase_green_time(effective2);

		res2 = remaining_green_time(effective2, green2);

		effective3 = remaining_effective_green_time(res2, ratio[2], ratio[1], 2);

		green3 = phase_green_time(effective3);

		res3 = remaining_green_time(effective3, green3);

		effective4 = remaining_effective_green_time(res3, ratio[3], ratio[2], 3);

		green4 = phase_green_time(effective4);
	}

  return 0;
}

int count_vehicle(int num_vehicle)
{
	return num_vehicle;
}

int current_traffic_volume(int num_vechicle, char C)
{
	int Q_RT;

//	Q_RT = (num_vechicle*3600)/C;
	Q_RT = (num_vechicle)/C;

	return Q_RT;
}

char flow_ratio(int Q_RT, int FS)
{
	char ratio;

	ratio = Q_RT/FS;

	return ratio;
}

int effective_green_time(char flow_ratio, char C)
{
	int VE;

	VE = flow_ratio*C;

	return VE;
}

int phase_green_time(int VE)
{
	int V;
	char P = 2;
	char G = 3;

	V = VE+P-G;

	return V;
}

int remaining_green_time(int VE, int V)
{
	int Vres;

	Vres = VE - V;

	return Vres;
}

int remaining_effective_green_time(int Vres, float flow_ratio_i, float flow_ratio_j, char n)
{
	char i;
	int VE_res;
	float flow_ratio_k;

	for(i = n; i < 4; i++)
	{
		flow_ratio_k += ratio[i];			// Got problem!!!!
	}

	VE_res = Vres*(flow_ratio_i/(flow_ratio_k-flow_ratio_j));

	return VE_res;
}

float compare(float first, float second, float third, float fourth)
{
	char i;
	float temp_value, final;
	char temp_priority;

	ratio[0] = first;
	ratio[1] = second;
	ratio[2] = third;
	ratio[3] = fourth;

	for(i = 0; i < 4; i++)
	{
		if (ratio[i] < ratio[i+1])
		{
			temp_value = ratio[i];
			ratio[i] = ratio[i+1];
			ratio[i+1] = temp_value;

			temp_priority = ratio_priority[i];
			ratio_priority[i] = ratio_priority[i+1];
			ratio_priority[i+1] = temp_priority;
		}
	}

	final = ratio[0];

	return final;
}

