﻿using System;

namespace Engine
{
    public interface ICollisionBehavior
    {
        void HandleCollision(Entity entity, Entity other);
    }
}

