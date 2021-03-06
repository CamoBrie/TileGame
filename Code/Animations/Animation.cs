﻿using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Documents;
using System;

namespace TileGame.Code.Animations
{
    internal class Animation
    {
        /// <summary>
        /// The current frame active in the animation
        /// </summary>
        internal AsepriteFrame currentFrame => frames[currentFrameIndex];

        /// <summary>
        /// The index of the current Frame
        /// </summary>
        internal int currentFrameIndex = 0;

        /// <summary>
        /// The tag associated with this animation
        /// </summary>
        internal string tag;

        /// <summary>
        /// An array of all the frames in this animation
        /// </summary>
        private readonly AsepriteFrame[] frames;

        /// <summary>
        /// The index of the first frame of this animation in the doc's list of frames, and the last
        /// </summary>
        private readonly int[] frameRange;

        /// <summary>
        /// The number of frames this animation has
        /// </summary>
        private int amountOfFrames => (frameRange[1] - frameRange[0]) + 1;

        /// <summary>
        /// The progress in seconds through the current frame
        /// </summary>
        private float frameProgress = 0.0f;

        /// <summary>
        /// Returns true if the animation is relapsing to it's first frame
        /// </summary>
        internal bool relapsing;

        /// <summary>
        /// Create an animation with a AsepriteDocument defined with a tag.
        /// </summary>
        /// <param name="doc">The document to derive the frames from</param>
        /// <param name="tag">A tag defining a range of frames within the document</param>
        internal Animation(ref AsepriteDocument doc, string tag)
        {
            this.tag = tag;

            ///Attempt to extract an Asetag from the doc with the tag
            AsepriteTag aseTag = doc.Tags[tag];
            if(aseTag != null)
            {
                //Get the framerange from the tag
                frameRange = new int[] { aseTag.From, aseTag.To};
                //Get the frames from the doc in the range
                frames = new AsepriteFrame[frameRange[1] - frameRange[0] + 1];
                for(int i = 0; i < frames.Length; i++)
                {
                    frames[i] = doc.Frames[frameRange[0] + i];
                }

                currentFrameIndex = 0;
            }
            else
            {
                Console.WriteLine("Tag: " + tag + ", does not exist!");
            }
        }

        /// <summary>
        /// Constructor for an animation with a doc with no tags (Only so the game doesn't crash, Use tags!)
        /// </summary>
        /// <param name="doc">The document to derive the frames from</param>
        internal Animation(ref AsepriteDocument doc)
        {
            tag = doc.Texture.Name;

            //Get the framerange from the doc
            frameRange = new int[] { 0, doc.Frames.Count-1 };
            //Get the frames from the doc in the range
            frames = new AsepriteFrame[frameRange[1] - frameRange[0] + 1];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = doc.Frames[frameRange[0] + i];
            }

            currentFrameIndex = 0;
        }

        /// <summary>
        /// Cycle through the frames
        /// </summary>
        /// <param name="time"></param>
        internal void Update(GameTime time)
        {
            relapsing = false;
            //Progress the frameProgress
            frameProgress += (float)time.ElapsedGameTime.TotalSeconds;
            //Check if the progress has passed the frameduration
            if(frameProgress >= currentFrame.Duration)
            {
                //Reset the frameprogress
                frameProgress -= currentFrame.Duration;
                if (currentFrameIndex == amountOfFrames - 1)
                {
                    currentFrameIndex = 0;
                    relapsing = true;
                }
                else
                {
                    currentFrameIndex++;
                }
            }
        }
    }
}
