﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
public class Fan : AnimationSprite
{
    public MyGame myGame;

    public float top;
    public float bottom;
    public float left;
    public float right;
    public float blowDistance;

    public Vec2 position;
    bool isLeft = false;
    bool active;
    bool switched = false;
    Button button;
    SoundChannel fanSFX;
    
    public Fan(string filename, int cols, int rows, Vec2 position, bool isLeft, float blowDistance, Button button = null, int frames = -1, bool keepInCache = false, bool addCollider = true) : base(filename, cols, rows, frames, keepInCache, addCollider)
    {
        myGame = (MyGame)game; 
        this.isLeft = isLeft;
        this.button = button;
        this.blowDistance = blowDistance;
        
        SetOrigin(width / 2, height / 2);
        SetXY(position.x, position.y);

        
       

        if (isLeft) { Mirror(true, false); }

        top = y - height / 2;
        bottom = y + height / 2;
        left = x - width / 2;
        right = x + width / 2;
    }

    public void Update()
    {
        
        if (button == null)
        {
            active = true;
        }
        else if (button.isActivated)
        {
            
            active = true;
        }
        else
        {
            active = false;
        }


        if (active)
        {
            if (!switched)
            {
                fanSFX = new Sound("sfx/25.wav", true).Play();
                myGame.soundChannels.Add(fanSFX);
                switched = true;
            }
            CollisionCheck();
        }
        else if (!active && switched)
        {
            fanSFX.Stop();
            myGame.soundChannels.Remove(fanSFX);
            switched = false;
        }
    }

    private void CollisionCheck()
    {
        foreach (RigidBody other in myGame.rigidBodies)
        {
            if(isLeft)
            {
                if (other.top > top && other.top < bottom && other.right < left && other.isPushable && x - other.x < blowDistance)
                {
                    other.acceleration.SetXY(-0.23f, other.acceleration.y);
                }
            }
            if(!isLeft)
            {
                if(other.top > top && other.top < bottom && other.left > right && other.isPushable && x - other.x > -blowDistance)
                {
                    other.acceleration.SetXY(0.23f,other.acceleration.y);
                }
            }
        }
    }

}
