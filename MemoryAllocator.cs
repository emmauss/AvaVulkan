using Silk.NET.Vulkan;

namespace AvaVulkan
{
    static class MemoryAllocator
    {
        private static Vk _vk;
        public unsafe static DeviceMemory AllocateDeviceMemory(
            Vk vk,
            Device device,
            PhysicalDevice physicalDevice,
            MemoryRequirements requirements,
            out long size,
            MemoryPropertyFlags flags = 0,
            bool isExternal =  false)
        {
            _vk = vk;

            size = 0;
            
            int memoryTypeIndex = FindSuitableMemoryTypeIndex(physicalDevice, requirements.MemoryTypeBits, flags);
            if (memoryTypeIndex < 0)
            {
                return default;
            }

            MemoryAllocateInfo info = new MemoryAllocateInfo()
            {
                SType = StructureType.MemoryAllocateInfo,
                AllocationSize = requirements.Size,
                MemoryTypeIndex = (uint) memoryTypeIndex
            };

            if (isExternal)
            {
                ExportMemoryAllocateInfo exInfo = new ExportMemoryAllocateInfo()
                {
                    HandleTypes = ExternalMemoryHandleTypeFlags.ExternalMemoryHandleTypeOpaqueWin32Bit | ExternalMemoryHandleTypeFlags.ExternalMemoryHandleTypeOpaqueFDBit
                };

                info.PNext = &exInfo;
            }

            size = (long) requirements.Size;

            var result = vk.AllocateMemory(device, &info, null, out var memory);

            if (result != Result.Success)
            {
                return default;
            }

            return memory;
        }

        private static int FindSuitableMemoryTypeIndex(PhysicalDevice physicalDevice, uint memoryTypeBits, MemoryPropertyFlags flags)
        {
            _vk.GetPhysicalDeviceMemoryProperties(physicalDevice, out var props);

            for (int i = 0; i < props.MemoryTypeCount; i++)
            {
                var type = props.MemoryTypes[i];

                if ((memoryTypeBits & (1 << i)) != 0 && type.PropertyFlags.HasFlag(flags))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}