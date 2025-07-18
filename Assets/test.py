from PIL import Image
import os

# === Cấu hình ===
INPUT_FOLDER = "source"    # Thư mục chứa ảnh gốc
OUTPUT_FOLDER = "FoodItem"  # Thư mục lưu ảnh sau khi crop
TARGET_RATIO = (28, 31)           # Tỉ lệ mong muốn (VD: 16:9)
OUTPUT_FORMAT = "PNG"           # Định dạng đầu ra: JPEG hoặc PNG

os.makedirs(OUTPUT_FOLDER, exist_ok=True)

def center_crop(image_path, output_path, target_ratio=(16, 9), output_format="JPEG"):
    img = Image.open(image_path)
    width, height = img.size

    # Tính tỉ lệ hiện tại và target
    current_ratio = width / height
    target_ratio_val = target_ratio[0] / target_ratio[1]

    # Xác định vùng crop
    if current_ratio > target_ratio_val:  # ảnh rộng hơn
        new_width = int(height * target_ratio_val)
        new_height = height
    else:  # ảnh cao hơn
        new_width = width
        new_height = int(width / target_ratio_val)

    # Crop từ trung tâm
    left = (width - new_width) // 2
    top = (height - new_height) // 2
    right = left + new_width
    bottom = top + new_height

    img_cropped = img.crop((left, top, right, bottom))

    # Lưu ảnh
    img_cropped.save(output_path, format=output_format, quality=95)
    print(f"✅ Saved: {output_path} | Size: {img_cropped.size}")


# === Batch xử lý ===
for file_name in os.listdir(INPUT_FOLDER):
    if file_name.lower().endswith(('.jpg', '.jpeg', '.png', '.bmp', '.webp')):
        input_path = os.path.join(INPUT_FOLDER, file_name)
        output_file = os.path.splitext(file_name)[0] + ".png"
        output_path = os.path.join(OUTPUT_FOLDER, output_file)

        center_crop(input_path, output_path, TARGET_RATIO, OUTPUT_FORMAT)

print("🎯 Hoàn tất crop tất cả ảnh!")
