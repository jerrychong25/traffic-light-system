function [final, final_priority] = compare(first, second, third, fourth)

    ratio = [0 0 0 0];
    ratio_priority = [1 2 3 4];

    ratio(1,1) = first;
	ratio(1,2) = second;
	ratio(1,3) = third;
	ratio(1,4) = fourth;
    
    for i = 1: 1: 3    
        for n = 1: 1: 3
            if (ratio(1,n) < ratio(1,n+1))
                temp_value = ratio(1,n);
                ratio(1,n) = ratio(1,n+1);
                ratio(1,n+1) = temp_value;

                temp_priority = ratio_priority(1,n);
                ratio_priority(1,n) = ratio_priority(1,n+1);
                ratio_priority(1,n+1) = temp_priority;
            end
        end
    end
           
    final = ratio;
    final_priority = ratio_priority;
end