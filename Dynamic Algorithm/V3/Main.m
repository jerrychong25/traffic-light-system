clc % clears cmd window
clear all % clears workspace
close all % closes figure

% Fixed Variables
fixed_number = [0 0 0 0];
fixed_waiting_time = [0 0 0 0];
total_average_fixed_waiting_time = zeros(1,101);
total_fixed_number = zeros(1,101);
total_average_fixed_number = zeros(1,101);
fixed_green_time = [75 75 75 75];

% Dynamic Variables
dynamic_number = [0 0 0 0];
dynamic_waiting_time = [0 0 0 0];
total_average_dynamic_waiting_time = zeros(1,101);
total_dynamic_number = zeros(1,101);
total_average_dynamic_number = zeros(1,101);
dynamic_priority = [0 0 0 0];
dynamic_green_time = [0 0 0 0];

% Other Variables
% 1,1 = North
% 1,2 = East
% 1,3 = South
% 1,4 = West
fixed_number_north = zeros(1,101);
fixed_number_east = zeros(1,101);
fixed_number_south = zeros(1,101);
fixed_number_west = zeros(1,101);
fixed_waiting_time_north = zeros(1,101);
fixed_waiting_time_east = zeros(1,101);
fixed_waiting_time_south = zeros(1,101);
fixed_waiting_time_west = zeros(1,101);
dynamic_number_north = zeros(1,101);
dynamic_number_east = zeros(1,101);
dynamic_number_south = zeros(1,101);
dynamic_number_west = zeros(1,101);
dynamic_waiting_time_north = zeros(1,101);
dynamic_waiting_time_east = zeros(1,101);
dynamic_waiting_time_south = zeros(1,101);
dynamic_waiting_time_west = zeros(1,101);
% percentage_fixed = zeros(1,101);
% percentage_dynamic = zeros(1,101);
% percentage_better = zeros(1,101);

for i = 2: 1: 101
    % Test Variables
    simulated_traffic = randi([0 50],1,4);
    
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

    fixed_waiting_time(1,1) = fixed_number(1,1)/3;          % Speed is 3 metres/second
    fixed_waiting_time(1,2) = fixed_number(1,2)/3;
    fixed_waiting_time(1,3) = fixed_number(1,3)/3;
    fixed_waiting_time(1,4) = fixed_number(1,4)/3;
    
    dynamic_number(1,dynamic_priority(1,1)) = fix(remaining_traffic(simulated_traffic_dynamic(1,1),dynamic_green_time(1,dynamic_priority(1,1))));
    dynamic_number(1,dynamic_priority(1,2)) = fix(remaining_traffic(simulated_traffic_dynamic(1,2),dynamic_green_time(1,dynamic_priority(1,2))));
    dynamic_number(1,dynamic_priority(1,3)) = fix(remaining_traffic(simulated_traffic_dynamic(1,3),dynamic_green_time(1,dynamic_priority(1,3))));
    dynamic_number(1,dynamic_priority(1,4)) = fix(remaining_traffic(simulated_traffic_dynamic(1,4),dynamic_green_time(1,dynamic_priority(1,4))));

    dynamic_waiting_time(1,1) = dynamic_number(1,1)/3;          % Speed is 3 metres/second
    dynamic_waiting_time(1,2) = dynamic_number(1,2)/3;
    dynamic_waiting_time(1,3) = dynamic_number(1,3)/3;
    dynamic_waiting_time(1,4) = dynamic_number(1,4)/3;
    
    % Save Data For Figures Plotting
    fixed_number_north(1,i) = fixed_number(1,1)*4.2;            % Average car length is 4.2 metres
    fixed_number_east(1,i) = fixed_number(1,2)*4.2;
    fixed_number_south(1,i) = fixed_number(1,3)*4.2;
    fixed_number_west(1,i) = fixed_number(1,4)*4.2;
    
    fixed_waiting_time_north(1,i) = fixed_waiting_time(1,1);
    fixed_waiting_time_east(1,i) = fixed_waiting_time(1,2);
    fixed_waiting_time_south(1,i) = fixed_waiting_time(1,3);
    fixed_waiting_time_west(1,i) = fixed_waiting_time(1,4);
    
    dynamic_number_north(1,i) = dynamic_number(1,1)*4.2;
    dynamic_number_east(1,i) = dynamic_number(1,2)*4.2;
    dynamic_number_south(1,i) = dynamic_number(1,3)*4.2;
    dynamic_number_west(1,i) = dynamic_number(1,4)*4.2;
    
    dynamic_waiting_time_north(1,i) = dynamic_waiting_time(1,1);
    dynamic_waiting_time_east(1,i) = dynamic_waiting_time(1,2);
    dynamic_waiting_time_south(1,i) = dynamic_waiting_time(1,3);
    dynamic_waiting_time_west(1,i) = dynamic_waiting_time(1,4);
    
    total_average_fixed_waiting_time(1,i) = sum(fixed_waiting_time)/4;
    total_average_dynamic_waiting_time(1,i) = sum(dynamic_waiting_time)/4;
    
    total_average_fixed_number(1,i) = sum(fixed_number)*4.2/4;
    total_average_dynamic_number(1,i) = sum(dynamic_number)*4.2/4;
    
    total_fixed_number(1,i) = sum(fixed_number)*4.2;
    total_dynamic_number(1,i) = sum(dynamic_number)*4.2;

%     percentage_fixed(1,i) = ((total_simulated_traffic_fixed-total_fixed_number)/total_simulated_traffic_fixed)*100
%     percentage_dynamic(1,i) = ((total_simulated_traffic_dynamic-total_dynamic_number)/total_simulated_traffic_dynamic)*100
% 
%     percentage_better(1,i) = percentage_dynamic - percentage_fixed

end

%   Individual Paths Figures For Traffic Queue Lengths
figure;
subplot(2,2,1)       % add first plot in 2 x 2 grid
x_number_north=0:100;
y_number_north=fixed_number_north;
z_number_north=dynamic_number_north;
fixed_plot_number_north = plot(0:length(x_number_north)-1,y_number_north);         % Plot line graph with fixed time algorithm
set(fixed_plot_number_north,'LineWidth',1);
set(fixed_plot_number_north,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_number_north,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of North Path Traffic Queue Lengths Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Traffic Queue Lengths (metres)')   % Y-axis label
hold;
dynamic_plot_number_north = plot(0:length(x_number_north)-1,z_number_north);       % Plot line graph with dynamic algorithm
set(dynamic_plot_number_north,'LineWidth',1);
set(dynamic_plot_number_north,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_number_north,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

subplot(2,2,2)       % add second plot in 2 x 2 grid
x_number_east=0:100;
y_number_east=fixed_number_east;
z_number_east=dynamic_number_east;
fixed_plot_number_east = plot(0:length(x_number_east)-1,y_number_east);         % Plot line graph with fixed time algorithm
set(fixed_plot_number_east,'LineWidth',1);
set(fixed_plot_number_east,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_number_east,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of East Path Traffic Queue Lengths Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Traffic Queue Lengths (metres)')   % Y-axis label
hold;
dynamic_plot_number_east = plot(0:length(x_number_east)-1,z_number_east);       % Plot line graph with dynamic algorithm
set(dynamic_plot_number_east,'LineWidth',1);
set(dynamic_plot_number_east,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_number_east,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

subplot(2,2,3)       % add third plot in 2 x 2 grid
x_number_south=0:100;
y_number_south=fixed_number_south;
z_number_south=dynamic_number_south;
fixed_plot_number_south = plot(0:length(x_number_south)-1,y_number_south);         % Plot line graph with fixed time algorithm
set(fixed_plot_number_south,'LineWidth',1);
set(fixed_plot_number_south,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_number_south,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of South Path Traffic Queue Lengths Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Traffic Queue Lengths (metres)')   % Y-axis label
hold;
dynamic_plot_number_south = plot(0:length(x_number_south)-1,z_number_south);       % Plot line graph with dynamic algorithm
set(dynamic_plot_number_south,'LineWidth',1);
set(dynamic_plot_number_south,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_number_south,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

subplot(2,2,4)       % add fourth plot in 2 x 2 grid
x_number_west=0:100;
y_number_west=fixed_number_west;
z_number_west=dynamic_number_west;
fixed_plot_number_west = plot(0:length(x_number_west)-1,y_number_west);         % Plot line graph with fixed time algorithm
set(fixed_plot_number_west,'LineWidth',1);
set(fixed_plot_number_west,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_number_west,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of West Path Traffic Queue Lengths Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Traffic Queue Lengths (metres)')   % Y-axis label
hold;
dynamic_plot_number_west = plot(0:length(x_number_west)-1,z_number_west);       % Plot line graph with dynamic algorithm
set(dynamic_plot_number_west,'LineWidth',1);
set(dynamic_plot_number_west,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_number_west,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

%   Individual Paths Figures For Waiting Time
figure;
subplot(2,2,1)       % add first plot in 2 x 2 grid
x_waiting_time_north=0:100;
y_waiting_time_north=fixed_waiting_time_north;
z_waiting_time_north=dynamic_waiting_time_north;
fixed_plot_waiting_time_north = plot(0:length(x_waiting_time_north)-1,y_waiting_time_north);         % Plot line graph with fixed time algorithm
set(fixed_plot_waiting_time_north,'LineWidth',1);
set(fixed_plot_waiting_time_north,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_waiting_time_north,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of North Path Waiting Time Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Waiting Time (seconds)')   % Y-axis label
hold;
dynamic_plot_waiting_time_north = plot(0:length(x_waiting_time_north)-1,z_waiting_time_north);       % Plot line graph with dynamic algorithm
set(dynamic_plot_waiting_time_north,'LineWidth',1);
set(dynamic_plot_waiting_time_north,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_waiting_time_north,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

subplot(2,2,2)       % add second plot in 2 x 2 grid
x_waiting_time_east=0:100;
y_waiting_time_east=fixed_waiting_time_east;
z_waiting_time_east=dynamic_waiting_time_east;
fixed_plot_waiting_time_east = plot(0:length(x_waiting_time_east)-1,y_waiting_time_east);         % Plot line graph with fixed time algorithm
set(fixed_plot_waiting_time_east,'LineWidth',1);
set(fixed_plot_waiting_time_east,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_waiting_time_east,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of East Path Waiting Time Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Waiting Time (seconds)')   % Y-axis label
hold;
dynamic_plot_waiting_time_east = plot(0:length(x_waiting_time_east)-1,z_waiting_time_east);       % Plot line graph with dynamic algorithm
set(dynamic_plot_waiting_time_east,'LineWidth',1);
set(dynamic_plot_waiting_time_east,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_waiting_time_east,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

subplot(2,2,3)       % add third plot in 2 x 2 grid
x_waiting_time_south=0:100;
y_waiting_time_south=fixed_waiting_time_south;
z_waiting_time_south=dynamic_waiting_time_south;
fixed_plot_waiting_time_south = plot(0:length(x_waiting_time_south)-1,y_waiting_time_south);         % Plot line graph with fixed time algorithm
set(fixed_plot_waiting_time_south,'LineWidth',1);
set(fixed_plot_waiting_time_south,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_waiting_time_south,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of South Path Waiting Time Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Waiting Time (seconds)')   % Y-axis label
hold;
dynamic_plot_waiting_time_south = plot(0:length(x_waiting_time_south)-1,z_waiting_time_south);       % Plot line graph with dynamic algorithm
set(dynamic_plot_waiting_time_south,'LineWidth',1);
set(dynamic_plot_waiting_time_south,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_waiting_time_south,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

subplot(2,2,4)       % add fourth plot in 2 x 2 grid
x_waiting_time_west=0:100;
y_waiting_time_west=fixed_waiting_time_west;
z_waiting_time_west=dynamic_waiting_time_west;
fixed_plot_waiting_time_west = plot(0:length(x_waiting_time_west)-1,y_waiting_time_west);         % Plot line graph with fixed time algorithm
set(fixed_plot_waiting_time_west,'LineWidth',1);
set(fixed_plot_waiting_time_west,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_waiting_time_west,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of West Path Waiting Time Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Waiting Time (seconds)')   % Y-axis label
hold;
dynamic_plot_waiting_time_west = plot(0:length(x_waiting_time_west)-1,z_waiting_time_west);       % Plot line graph with dynamic algorithm
set(dynamic_plot_waiting_time_west,'LineWidth',1);
set(dynamic_plot_waiting_time_west,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_waiting_time_west,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

%   Overall Paths Figures For Total Average Waiting Time
figure;
x_total_average_waiting_time=0:100;
y_total_average_waiting_time=total_average_fixed_waiting_time;
z_total_average_waiting_time=total_average_dynamic_waiting_time;
fixed_plot_total_average_waiting_time = plot(0:length(x_total_average_waiting_time)-1,y_total_average_waiting_time);         % Plot line graph with fixed time algorithm
set(fixed_plot_total_average_waiting_time,'LineWidth',1);
set(fixed_plot_total_average_waiting_time,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_total_average_waiting_time,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of Total Average Waiting Time Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Average Waiting Time (seconds)')   % Y-axis label
hold;
dynamic_plot_total_average_waiting_time = plot(0:length(x_total_average_waiting_time)-1,z_total_average_waiting_time);       % Plot line graph with dynamic algorithm
set(dynamic_plot_total_average_waiting_time,'LineWidth',1);
set(dynamic_plot_total_average_waiting_time,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_total_average_waiting_time,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

%   Overall Paths Figures For Total Average Traffic Queue Lengths
figure;
x_total_average_number=0:100;
y_total_average_number=total_average_fixed_number;
z_total_average_number=total_average_dynamic_number;
fixed_plot_total_average_number = plot(0:length(x_total_average_number)-1,y_total_average_number);         % Plot line graph with fixed time algorithm
set(fixed_plot_total_average_number,'LineWidth',1);
set(fixed_plot_total_average_number,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_total_average_number,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of Total Average Traffic Queue Lengths Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Average Queue Lengths (metres)')   % Y-axis label
hold;
dynamic_plot_total_average_number = plot(0:length(x_total_average_number)-1,z_total_average_number);       % Plot line graph with dynamic algorithm
set(dynamic_plot_total_average_number,'LineWidth',1);
set(dynamic_plot_total_average_number,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_total_average_number,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')

%   Overall Paths Figures For Total Traffic Queue Lengths
figure;
x_total_number=0:100;
y_total_number=total_fixed_number;
z_total_number=total_dynamic_number;
fixed_plot_total_number = plot(0:length(x_total_number)-1,y_total_number);         % Plot line graph with fixed time algorithm
set(fixed_plot_total_number,'LineWidth',1);
set(fixed_plot_total_number,'LineStyle','-');
% set(fixed_plot,'Marker','o');
set(fixed_plot_total_number,'Color','r');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
title('Graph of Total Traffic Queue Lengths Using Fixed Time Algorithm And Dynamic Algorithm')
xlabel('Traffic Light Cycles')    % X-axis label
ylabel('Traffic Queue Lengths (metres)')   % Y-axis label
hold;
dynamic_plot_total_number = plot(0:length(x_total_number)-1,z_total_number);       % Plot line graph with dynamic algorithm
set(dynamic_plot_total_number,'LineWidth',1);
set(dynamic_plot_total_number,'LineStyle','-');
% set(dynamic_plot,'Marker','s');
set(dynamic_plot_total_number,'Color','b');
% set(gca,'XTick',[0 1 2 3 4 5])
set(gca,'XTick',[0:10:100])
legend('Fixed Time','Dynamic','Location','northwest')
% figure
% stem(x,y);
% figure
% scatter(x,y)
