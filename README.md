# GraphGram
GraphGram is a tool that allows you to generate graphs from existing data, draw trendlines that take into account the uncertainties of the data, and calculate the coefficients of these trendlines along with their uncertainties. GraphGram has been developed particularly with IB students in mind, but other people can find it useful just as well.

This tool was originally created for a school project. The way in which it analyzes data is the same way that my physics teacher required me and my classmates to do it.

# The analysis algorithm

The algorithm that produces the lines that are drawn and the values of the gradients, y-intercepts, and their uncertainties works in the following way: It brute forces through all possible lines that can be drawn between two "corners" of the data points, groups them based on how many outliers they leave, or in other words - based on how many data points they pass through, it looks at the group with the least outliers, and picks those lines which have a set of outliers that occurs most often (e.g. if among all those lines, four of them don't pass through points no. 1, 3, and 7, and seven of them don't pass through 2, 8, and 9, then the second group gets chosen). It then picks the most and the least steep lines from those.

# Presentation

https://github.com/user-attachments/assets/3825a376-1aba-47ce-adde-07d05ecdf534
