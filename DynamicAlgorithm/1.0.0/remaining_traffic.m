function rt = remaining_traffic(traffic, green_time)

    number_vehicles_passby = green_time/3;

    if (traffic < number_vehicles_passby)
        rt = 0;
    else
        rt = traffic - number_vehicles_passby;
    end
end