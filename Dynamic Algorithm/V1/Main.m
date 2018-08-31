clc % clears cmd window
clear all % clears workspace
close all % closes figure

% Test Variables
simulated_traffic = randi([10 50],1,4)
total_simulated_traffic = sum(simulated_traffic)
fixed_number = [0 0 0 0];
total_fixed_number = 0;
fixed_green_time = [75 75 75 75];
dynamic_number = [0 0 0 0];
total_dynamic_number = 0;
dynamic_priority = [0 0 0 0];
dynamic_green_time = [0 0 0 0];

[dynamic_green_time, dynamic_priority] = Dynamic(simulated_traffic);

fixed_number(1,1) = remaining_traffic(simulated_traffic(1,1),fixed_green_time(1,1));
fixed_number(1,2) = remaining_traffic(simulated_traffic(1,2),fixed_green_time(1,2));
fixed_number(1,3) = remaining_traffic(simulated_traffic(1,3),fixed_green_time(1,3));
fixed_number(1,4) = remaining_traffic(simulated_traffic(1,4),fixed_green_time(1,4))

dynamic_number(1,dynamic_priority(1,1)) = fix(remaining_traffic(simulated_traffic(1,1),dynamic_green_time(1,dynamic_priority(1,1))));
dynamic_number(1,dynamic_priority(1,2)) = fix(remaining_traffic(simulated_traffic(1,2),dynamic_green_time(1,dynamic_priority(1,2))));
dynamic_number(1,dynamic_priority(1,3)) = fix(remaining_traffic(simulated_traffic(1,3),dynamic_green_time(1,dynamic_priority(1,3))));
dynamic_number(1,dynamic_priority(1,4)) = fix(remaining_traffic(simulated_traffic(1,4),dynamic_green_time(1,dynamic_priority(1,4))))

total_fixed_number = sum(fixed_number)
total_dynamic_number = sum(dynamic_number)

percentage_fixed = ((total_simulated_traffic-total_fixed_number)/total_simulated_traffic)*100
percentage_dynamic = ((total_simulated_traffic-total_dynamic_number)/total_simulated_traffic)*100

percentage_better = percentage_dynamic - percentage_fixed
