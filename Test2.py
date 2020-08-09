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
#from memorpy import *
import os
import subprocess  
import numpy as np
#import pyautogui
import win32gui, win32ui, win32con, win32api
import clr
from MemoryReadDotNet import MemoryRead
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

prisoner_template = cv2.imread('prisoner_pc_template.jpg',0)
game_over_template = cv2.imread('continue_pc.jpg',0)
#print(type(prisoner_template))
#zero_template = cv2.imread('0.jpg',0)
#one_template = cv2.imread('1.jpg',0)
#two_template = cv2.imread('2.jpg',0)

def launch_game():
#    r2 = os.chdir("D:/Telechargement/METAL.SLUG-ALI213/METAL SLUG")
#    output =subprocess.Popen(['csume','mslugx','-resolution','640x480'])
    output =subprocess.Popen('D:/Telechargement/METAL.SLUG-ALI213/METAL SLUG/mslug1.exe')
    time.sleep(1)
#
launch_game()
    


#get_ready()


#mw=MemWorker(pid=8208)
#l=[x for x in mw.umem_search("hello")]
#a = l[0]
#l[0].dump()
#a.read(100).decode("utf-16-le")
#last = time.time()
#mw.umem_replace("hello","pwned")
def move_window_game_left():    
    time.sleep(1)
    hwnd = win32gui.GetForegroundWindow()
    win32gui.MoveWindow(hwnd, 0, 0, 800, 600, True)
    
def move_window_capture_right():    
    cv2.namedWindow("window")
    cv2.moveWindow("window",900,0)

def set_window_game_foreground():
    window = win32gui.FindWindow(None, "UME: Metal Slug X - Super Vehicle-001 (NGM-2500)(NGH-2500) [mslugx]")
    win32gui.SetForegroundWindow(window)

#prisoner_template = cv2.imread('prisoner_template.jpg',0)

    
move_window_game_left()

#move_window_capture_right()
#set_window_game_foreground()
    
    
def grab_screen(region=None):

    hwin = win32gui.GetDesktopWindow()

    if region:
            left,top,x2,y2 = region
            width = x2 - left + 1
            height = y2 - top + 1
    else:
        width = win32api.GetSystemMetrics(win32con.SM_CXVIRTUALSCREEN)
        height = win32api.GetSystemMetrics(win32con.SM_CYVIRTUALSCREEN)
        left = win32api.GetSystemMetrics(win32con.SM_XVIRTUALSCREEN)
        top = win32api.GetSystemMetrics(win32con.SM_YVIRTUALSCREEN)

    hwindc = win32gui.GetWindowDC(hwin)
    srcdc = win32ui.CreateDCFromHandle(hwindc)
    memdc = srcdc.CreateCompatibleDC()
    bmp = win32ui.CreateBitmap()
    bmp.CreateCompatibleBitmap(srcdc, width, height)
    memdc.SelectObject(bmp)
    memdc.BitBlt((0, 0), (width, height), srcdc, (left, top), win32con.SRCCOPY)
    
    signedIntsArray = bmp.GetBitmapBits(True)
    img = np.frombuffer(signedIntsArray, dtype='uint8')
    img.shape = (height,width,4)

    srcdc.DeleteDC()
    memdc.DeleteDC()
    win32gui.ReleaseDC(hwin, hwindc)
    win32gui.DeleteObject(bmp.GetHandle())

    return cv2.cvtColor(img, cv2.COLOR_BGRA2RGB)    

#prisoners = []
current_state = []
is_game_over = []
NUMBER_FRAMES = 0
gray = []
def show_capture():
    global NUMBER_FRAMES
    global gray
#    test = True
#    prisoner_template = cv2.imread('prisoner_template.jpg',0)
    while True:
#        prisoners.clear()
#        first = []
#        last = time.time()
        current_state.clear()
        prisoners = []
        
        screen = grab_screen(region=(5,30,800,629))
#        screen = pyautogui.screenshot(region=(0,0, 640, 480))
        gray = cv2.cvtColor(screen,cv2.COLOR_BGR2GRAY)
#        prisoners_area = cv2.rectangle(gray,(43,551),(340,575),(0,255,0),1)#prisoners(max = 14)
        current_state.append(gray)
        prisoners_area = cv2.rectangle(gray,(43,551),(340,575),(0,255,0),1)
#        w, h = prisoner_template.shape[::-1]
        game_over_area = cv2.rectangle(gray,(210,173),(593,217),(0,255,0),1)
        res_prisoners = cv2.matchTemplate(prisoners_area,prisoner_template,cv2.TM_CCOEFF_NORMED)
        res_game_over = cv2.matchTemplate(game_over_area,game_over_template,cv2.TM_CCOEFF_NORMED)
        threshold = 0.8
        locp = np.where( res_prisoners >= threshold)
        locgameover = np.where(res_game_over >= threshold)
#        print(len(locp))
#        print(locp[::-1])
        
        
        for pt in zip(*locp[::-1]):            
            prisoners.append(1)
#            print(len(prisoners))
        print(len(prisoners))
        
        for pt in zip(*locgameover[::-1]):
            is_game_over.append(1)
            
#        if(len(is_game_over)>0):
#            print("game over ! ")

            
        
        
        
        
#        test = zip(*loc[::-1])
#        print(list(test))
        cv2.imshow("window",gray)
        NUMBER_FRAMES += 1
#        key2 = cv2.waitKey(25) & 0xFF
#        if key2 == ord('t'):
#        print(type(screen))
#        print(type(prisoner_template))
#        prisoner_template = np.asarray(prisoner_template)
#        print(type(gray))
#        screen
        
#            print('t pressed !')
        
    #    at.PressKey(at.L)
    #    time.sleep(0.025)
    #    at.ReleaseKey(at.L)
        key = cv2.waitKey(25) & 0xFF
        if key == ord('x'):
#            test = False
            cv2.destroyAllWindows()
            cv2.waitKey(1)
            break

#import random

def step(action):
    global NUMBER_FRAMES
    global gray
    next_state = []
    is_done = False
    current_frames = NUMBER_FRAMES
    if(action =="up"):
        Inputs.up()
    if(action =="down"):
        Inputs.down()
    if(action =="left"):
        Inputs.left()
    if(action =="right"):
        Inputs.right()
    if(action =="fire"):
        Inputs.fire()
    if(action =="grenade"):
        Inputs.grenade()
    if(action =="jump"):
        Inputs.jump()
#    if(NUMBER_FRAMES % 4 == 0):
        #Final reward function = ax + by + cz
        #a => lives
        #b => score/ seconds
        #c => prisoners
        #x >> y > z
    reward = read_score()
    #lives = read_lives
    #prisoners = read_prisoners
    if(len(is_game_over)>0):
            is_done = True
        
    while(NUMBER_FRAMES - current_frames != 3):
        next_state.append(gray)
        
        
    next_state = np.array(next_state)
    return next_state,reward,is_done

def reset():
    
    
    
        


        
    


#proc_id = get_game_pid("csume.exe")
#def match_prisoners():
#    cv2.rectangle(gray,(18,156),(285,176),(0,255,0),1)#prenom
#print(proc_id)
#mw=MemWorker(pid=proc_id)
#l=[x for x in mw.umem_search("CREDIT")]
#l[0].dump()
#time.sleep(6)
#lo=Locator(mw)
#print(lo.feed(10))

#import os      
#prisoner_template = cv2.imread('prisoner_template.jpg',0)
#screen = pyautogui.screenshot(region=(0,0, 640, 480))
#gray = cv2.cvtColor(np.array(screen),cv2.COLOR_BGR2GRAY)
#res = cv2.matchTemplate(gray,prisoner_template,cv2.TM_CCOEFF_NORMED)
#print(type(gray))
#prisoner_template = np.asarray(prisoner_template)
#print(type(Inputs.prisoner_template))


#res = cv2.matchTemplate(gray,prisoner_template,cv2.TM_CCOEFF_NORMED)


show_capture()
#import ctypes
#score = ctypes.cdll.LoadLibrary(R"C:\Users\Smail\source\repos\MemoryReadDotNet\MemoryReadDotNet\bin\Debug\MemoryReadDotNet.dll")
#score.Read_Score()

#import os
#print(str(os.getcwd()) +"\MemoryReadDotNet.dll")


def read_score():
    ref = clr.AddReference(os.getcwd() +"\MemoryReadDotNet.dll")
#    from MemoryReadDotNet import MemoryRead
    print(MemoryRead.Read_Score())
    
#read_score()
    





    

