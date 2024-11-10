import psutil

def get_system_usage():
    # Uso de CPU
    cpu_usage = psutil.cpu_percent(interval=1)

    # Uso de memoria
    memory_info = psutil.virtual_memory()
    ram_usage = memory_info.percent

    # Información de swap (usualmente se usa como buffer adicional)
    swap_info = psutil.swap_memory()
    buffer_usage = swap_info.percent

    print(f"Uso de CPU: {cpu_usage}%")
    print(f"Uso de RAM: {ram_usage}%")
    print(f"Uso de Buffer (Swap): {buffer_usage}%")

if __name__ == "__main__":
    get_system_usage()
