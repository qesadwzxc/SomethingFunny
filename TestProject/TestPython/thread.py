#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import thread
import time
lk = thread.allocate_lock()
g_FinishCount = 0
def loop(id):
    lk.acquire()  #申请锁
    for i in range (0,4):
        print "Thread ",id," working"
        time.sleep(1)
    lk.release()  #释放锁
    global g_FinishCount
    g_FinishCount = g_FinishCount + 1

thread.start_new_thread(loop,(1,))
thread.start_new_thread(loop,(2,))
thread.start_new_thread(loop,(3,))
while g_FinishCount < 3:
    time.sleep(1)
