#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import os
print(os.getcwd());

# 不加.0会被认为是int型导致除法结果会社区小数点后
# 若想小数除法取整可以用'//'代替'/'
lastScore=45.0;
thisScore=85.0;
upgradePercent=(thisScore-lastScore)/lastScore;
# 加了声明控制台依然是乱码，因为windows的本地默认编码是cp936也就是gbk编码
# 所以在控制台直接打印utf-8的字符串是乱码。解决方法：前面加'u'即可
print("提升的百分点为:%.1f%%" % (upgradePercent));

# input读取时会自动判断类型，raw_input则一改视为字符型
# python3去掉了raw_input方法同时input一改读取为字符型
# input("x:");
# raw_input("y:");

# if...else语句
if lastScore==45:
    print("if complete");
else:
    print("false");

# for循环
for i in range(1,100):
    if i==99:
        print("for complete");

# while循环
i=0;
while i<100:
    i=i+1;
    if i==100:
        print("while complete");

