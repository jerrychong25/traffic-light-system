clc % clears cmd window
clear all % clears workspace
close all % closes figure

fixed_number = [0 0 0 0];
total_fixed_number = zeros(1,1001);
fixed_green_time = [75 75 75 75];
dynamic_number = [0 0 0 0];
total_dynamic_number = zeros(1,1001);
dynamic_priority = [0 0 0 0];
dynamic_green_time = [0 0 0 0];
percentage_fixed = zeros(1,1001);
percentage_dynamic = zeros(1,1001);
percentage_better = zeros(1,1001);

for i = 2: 1: 1001
    % Test Variables
    simulated_traffic = randi([10 50],1,4);
    
    simulated_traffic_fixed = simulated_traffic;
    simulated_traffic_dynamic = simulated_traffic;
    
    if (i > 2)
        simulated_traffic_fixed(1,1) = simulated_traffic_fixed(1,1) + fixed_number(1,1);
        simulated_traffic_fixed(1,2) = simulated_traffic_fixed(1,2) + fixed_number(1,2);
        simulated_traffic_fixed(1,3) = simulated_traffic_fixed(1,3) + fixed_number(1,3);
        simulated_traffic_fixed(1,4) = simulated_traffic_fixed(1,4) + fixed_number(1,4);
        
        simulated_traffic_dynamic(1,1) = simulated_traffic_dynamic(1,1) + dynamic_number(1,1);
        simulated_traffic_dynamic(1,2) = simulated_traffic_dynamic(1,2) + dynamic_number(1,2);
        simulated_traffic_dynamic(1,3) = simulated_traffic_dynamic(1,3) + dynamic_number(1,3);
        simulated_traffic_dynamic(1,4) = simulated_traffic_dynamic(1,4) + dynamic_number(1,4);
    end
    
    total_simulated_traffic_fixed = sum(simulated_traffic_fixed);
    total_simulated_traffic_dynamic = sum(simulated_traffic_dynamic);
    
    [dynamic_green_time, dynamic_priority] = Dynamic(simulated_traffic_dynamic);

    fixed_number(1,1) = remaining_traffic(simulated_traffic_fixed(1,1),fixed_green_time(1,1));
    fixed_number(1,2) = remaining_traffic(simulated_traffic_fixed(1,2),fixed_green_time(1,2));
    fixed_number(1,3) = remaining_traffic(simulated_traffic_fixed(1,3),fixed_green_time(1,3));
    fixed_number(1,4) = remaining_traffic(simulated_traffic_fixed(1,4),fixed_green_time(1,4));

    dynamic_number(1,dynamic_priority(1,1)) = fix(remaining_traffic(simulated_traffic_dynamic(1,1),dynamic_green_time(1,dynamic_priority(1,1))));
    dynamic_number(1,dynamic_priority(1,2)) = fix(remaining_traffic(simulated_traffic_dynamic(1,2),dynamic_green_time(1,dynamic_priority(1,2))));
    dynamic_number(1,dynamic_priority(1,3)) = fix(remaining_traffic(simulated_traffic_dynamic(1,3),dynamic_green_time(1,dynamic_priority(1,3))));
    dynamic_number(1,dynamic_priority(1,4)) = fix(remaining_traffic(simulated_traffic_dynamic(1,4),dynamic_green_time(1,dynamic_priority(1,4))));

    total_fixed_number(1,i) = sum(fixed_number);
    total_dynamic_number(1,i) = sum(dynamic_number);

%     percentage_fixed(1,i) = ((total_simulated_traffic_fixed-total_fixed_number)/total_simulated_traffic_fixed)*100
%     percentage_dynamic(1,i) = ((total_simulated_traffic_dynamic-total_dynamic_number)/total_simulated_traffic_dynamic)*100
% 
%     percentage_better(1,i) = percentage_dynamic - percentage_fixed

end

x=0:1000;
y=total_fixed_number;
z=total_dynamic_number;
fixed_plot = plot(0:length(x)-1,y);         % Plot line graph with fixed time algorithm
set(fixed_plot,'LineWidth',1);
set(fixed_plot,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:100:1000])
title('Graph of Traffic Junction Queue Lengths Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Traffic Queue Lengths')   % Y-axis label
hold;
dynamic_plot = plot(0:length(x)-1,z);       % Plot line graph with dynamic algorithm
set(dynamic_plot,'LineWidth',1);
set(dynamic_plot,'LineStyle',':');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:100:1000])
legend('Fixed Time Algorithm','Dynamic Algorithm')
% figure
% stem(x,y);
% figure
% scatter(x,y)
