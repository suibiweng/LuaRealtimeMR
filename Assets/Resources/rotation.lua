-- Function to rotate an object in Unity
function rotate_object(proxy, speed, deltaTime)
    print("🔄 Rotating object in Lua") -- Debugging Lua execution
    local rotation = proxy:get_rotation()  -- Get current rotation
    rotation.y = rotation.y + speed * deltaTime  -- Rotate around Y-axis
    proxy:set_rotation(rotation)  -- Apply new rotation
end
