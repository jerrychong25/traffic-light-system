function VE_res = remaining_effective_green_time(Vres, ratio, flow_ratio_i, flow_ratio_j, n)

    flow_ratio_k = 0;
    
    for a = n: 1: 4    
        flow_ratio_k = flow_ratio_k + ratio(1,a);    % Got problem!!! 
    end
    
    if (n == 3)
        VE_res = fix(Vres);
    else
%         flow_ratio_k
%         flow_ratio_i
%         flow_ratio_j
%         Vres
        VE_res = round(Vres*(flow_ratio_i/(flow_ratio_k-flow_ratio_j)));
    end
end