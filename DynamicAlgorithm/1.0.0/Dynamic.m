function [calculated_green, ratio_priority] = Dynamic(junction_traffic)

% Variables
max_ratio = 0;
ratio_temp = [0 0 0 0];
ratio_priority = [0 0 0 0];
junc = [0 0 0 0];
traffic_volume = [0 0 0 0];
remaining_cycle = [0 0 0 0];
effective = [0 0 0 0];
green = [0 0 0 0];
res = [0 0 0];
cycle = 300;						% One Complete Cycle = 300 Seconds
max_flow_rate = 100;

% junc(1,1) = 60;
% junc(1,2) = 40;
% junc(1,3) = 30;
% junc(1,4) = 90;

junc(1,1) = junction_traffic(1,1);
junc(1,2) = junction_traffic(1,2);
junc(1,3) = junction_traffic(1,3);
junc(1,4) = junction_traffic(1,4);

traffic_volume(1,1) = current_traffic_volume(junc(1,1), cycle);
traffic_volume(1,2) = current_traffic_volume(junc(1,2), cycle);
traffic_volume(1,3) = current_traffic_volume(junc(1,3), cycle);
traffic_volume(1,4) = current_traffic_volume(junc(1,4), cycle)

ratio1 = flow_ratio(traffic_volume(1,1), max_flow_rate);
ratio2 = flow_ratio(traffic_volume(1,2), max_flow_rate);
ratio3 = flow_ratio(traffic_volume(1,3), max_flow_rate);
ratio4 = flow_ratio(traffic_volume(1,4), max_flow_rate);

ratio_temp(1,1) = ratio1/(ratio1+ratio2+ratio3+ratio4);
ratio_temp(1,2) = ratio2/(ratio1+ratio2+ratio3+ratio4);
ratio_temp(1,3) = ratio3/(ratio1+ratio2+ratio3+ratio4);
ratio_temp(1,4) = ratio4/(ratio1+ratio2+ratio3+ratio4);

[max_ratio, ratio_priority] = compare(ratio_temp(1,1), ratio_temp(1,2), ratio_temp(1,3), ratio_temp(1,4))		% Max Ratio

ratio_temp = max_ratio

% Calculation of first priority road bound

effective(1,1) = effective_green_time(ratio_temp(1,1), cycle);

green(1,1) = phase_green_time(effective(1,1));

res(1,1) = remaining_green_time(cycle, green(1,1));

remaining_cycle(1,1) = res(1,1);

% Calculation of second priority road bound

effective(1,2) = remaining_effective_green_time(res(1,1), ratio_temp, ratio_temp(1,2), ratio_temp(1,1), 1);

green(1,2) = phase_green_time(effective(1,2));

res(1,2) = remaining_green_time(res(1,1), green(1,2));

remaining_cycle(1,2) = res(1,2);

% Calculation of third priority road bound

effective(1,3) = remaining_effective_green_time(res(1,2), ratio_temp, ratio_temp(1,3), ratio_temp(1,2), 2);

green(1,3) = phase_green_time(effective(1,3));

res(1,3) = remaining_green_time(res(1,2), green(1,3))

remaining_cycle(1,3) = res(1,3);

% Calculation of fourth priority road bound

effective(1,4) = remaining_effective_green_time(res(1,3), ratio_temp, ratio_temp(1,4), ratio_temp(1,3), 3);

green(1,4) = phase_green_time(effective(1,4))

remaining_cycle(1,4) = remaining_cycle(1,3) - green(1,4)

calculated_green = green

end
