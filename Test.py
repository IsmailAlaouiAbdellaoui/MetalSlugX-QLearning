# -*- coding: utf-8 -*-
"""
Created on Wed Aug 22 22:08:52 2018

@author: smail
"""
import psutil
#import OpenMslugx
#from . import Inputs
import Inputs
#from pynput.keyboard import Key, Controller
from pynput import keyboard
#import numpy as np
#keyboard = Controller()
import cv2
import win32gui
#import MemWorker
from memorpy import *

import time

def on_press(key):
    try:
       cv2.destroyAllWindows()
    except AttributeError:
        print('special key {0} pressed'.format(
            key))

def on_release(key):
    print('{0} released'.format(
        key))
    if key == keyboard.Key.esc:
        # Stop listener
        return False


def get_ready():
    time.sleep(8)
    Inputs.PressKey(Inputs.ENTER)
    print("enter pressed !")
    time.sleep(0.5)
    Inputs.ReleaseKey(Inputs.ENTER)
    time.sleep(2)
    Inputs.PressKey(Inputs.Q)
    print("Q pressed !")
    time.sleep(2)
    Inputs.ReleaseKey(Inputs.Q)
    Inputs.PressKey(Inputs.Q)
    print("Q pressed !")
    time.sleep(1)
    Inputs.ReleaseKey(Inputs.Q)
    #time.sleep(8)
    #at.PressKey(at.L)
    #time.sleep(3)
    #at.ReleaseKey(at.L)
    
def get_game_pid(process_name):
    for proc in psutil.process_iter():
        if proc.name() == process_name:
            return proc.pid
    

#pid = get_game_pid("notepad.exe")
#print(pid)
get_ready()
#mw=MemWorker(pid=8808)
#l=[x for x in mw.umem_search("hello")]
#last = time.time()
def move_window_game_left():    
    time.sleep(1)
    hwnd = win32gui.GetForegroundWindow()
    win32gui.MoveWindow(hwnd, 0, 0, 640, 480, True)
    
def move_window_capture_right():    
    cv2.namedWindow("window")
    cv2.moveWindow("window",900,0)

def set_window_game_foreground():
    window = win32gui.FindWindow(None, "UME: Metal Slug X - Super Vehicle-001 (NGM-2500)(NGH-2500) [mslugx]")
    win32gui.SetForegroundWindow(window)
    
move_window_game_left()
move_window_capture_right()
set_window_game_foreground()

def show_capture():
    while 1:
#        last = time.time()
        screen = Inputs.grab_screen(region=(0,0,640,480))
        gray = cv2.cvtColor(screen,cv2.COLOR_BGR2GRAY)
        cv2.imshow("window",gray)
    #    at.PressKey(at.L)
    #    time.sleep(0.025)
    #    at.ReleaseKey(at.L)
        key = cv2.waitKey(25) & 0xFF
        if key == ord('x'):
            cv2.destroyAllWindows()
            cv2.waitKey(1)
            break

show_capture()
#import os      






    

