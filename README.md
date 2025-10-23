# GraphGram

GraphGram is a tool that allows you to generate graphs based on data you provide, draw lines of best fit that take into account the uncertainties of the data, and calculate their gradients and y-intercepts (along with their uncertainties), positions of any potential outliers, as well as coefficients of the steepest and the least steep lines.

This tool was originally created for a school project. The way in which it analyzes data is the same way that my physics teacher required me and my classmates to do it.

# The analysis algorithm

The algorithm that produces the lines that are drawn and the values of the gradients, y-intercepts, and their uncertainties works in the following way: It brute forces through all possible lines that can be drawn between two "corners" of the data points, groups them based on how many outliers they leave, or in other words - based on how many data points they pass through, it looks at the group with the least outliers, and picks those lines which have a set of outliers that occurs most often (e.g. if among the lines in the chosen group, four of them don't pass through points no. 1, 3, and 7, and seven of them don't pass through 2, 8, and 9, then the second subgroup gets chosen). It then picks the most and the least steep lines from those.

# Presentation

https://github.com/user-attachments/assets/3825a376-1aba-47ce-adde-07d05ecdf534

# How do I use it?

All you need to do is go to the Graph tab, put all of your data into the table on the left-hand side, and click Enter while having one of the table cells selected. GraphGram will take your data and perform all of the necessary calculations for you. The results of these calculations will take the form of a graph placed to the right of the data table, and a list of values in the Analysis tab.

You can choose what exactly gets drawn on the graph - you can toggle the visibility of the line of best fit, the steepest line, and the least steep line. You can also choose whether the calculations on the data should be performed using the data points' error bars or the error boxes. The check boxes to make these choices are placed below the data table.

If you want to easily share your datasets with others or import data into your table then go to the Import Export tab. There you will find the appropriate buttons for each action. Note: GraphGram exports data in a specific format and expects the files it imports to be in that format as well. The format is the following: numbers expressed just as they would be in the table with a Tab ("\t") after each number (except the last number in a row) and an Enter ("\r", "\n", or "\r\n") after each row.

## Rules of the Data Table

When GraphGram reads the data from the data table it follows a certain set of rules. It goes row by row, starting from the top and working its way down from there. When reading a row, it determines whether the data in this row is valid, and for it to be so, the cells must contain a real number and use a period (.) as the decimal separator. Also, the uncertainties must be non-negative. If the data in at least one of the cells in a row fails to follow these rules, the whole row is rendered invalid. GraphGram stops reading the data table after encountering an invalid row, meaning that if you have 10 rows of data and there is an invalid cell in the 6th row, only the first 5 rows will be read. If less than 2 rows get marked as valid, nothing will be drawn on the graph.

## Tips and Tricks

### Change column titles

You can change the titles of the data table's x and y columns. Click on the cell with the title you'd like to change, click it again to select it, write your preferred title, and confirm by clicking Enter. Your change will also be automatically applied to the corresponding uncertainty title and to the graph.

### Add Exponents to Titles

You can add exponents to the titles of the data table's columns by adding ^(...) to the title and putting your desired exponent between the braces. This will raise the content between the braces slightly higher and make the font smaller. You can add more than one exponent in a single title. For example, your title can be x^(2), abc^(def) or n^(q)m^(p). Keep in mind that exponents can't be put inside other exponents.

### Write Shorter Inputs

When your data consists of very large or very small numbers, putting them into GraphGram may make the program look cluttered. For example, the numbers on the x-axis may overlap, or the numbers on the y-axis may stick out of the graph's area. In order to fix this, it is advised to change the units of the data in the data table. By this it is meant that a number like 0.00000075, which is expressed in meters, can be converted to 750 nanometers. Be careful! Changing the units of your data will also have an effect on the calculated values in the Analysis tab, which you will have to take into account when doing any calculations later on.

### Change the Theme

GraphGram is available in both the light and the dark theme. The theme automatically adjusts to your system theme.
