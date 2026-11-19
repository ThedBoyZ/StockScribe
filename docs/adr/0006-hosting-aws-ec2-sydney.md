# ADR-0006 · โฮสต์บน AWS EC2 t3.micro ที่ Sydney แทน Google Compute Engine

- สถานะ: Accepted · 20 พ.ย. 2026
- แทนที่: ส่วน "Deploy" และ "Region" ใน [architecture.md](../sa/architecture.md) ฉบับ 17 พ.ย. 2026 (GCE e2-micro, us-west1)

## บริบท

แผนเดิมใช้ VM e2-micro ของ Google Compute Engine ที่ us-west1 ร่วมกับ Neon และ Atlas ที่ Oregon แต่ตอนสมัครเจอปัญหา

- GCP ต้องจ่ายล่วงหน้า (prepayment) 1,000 บาทก่อนจึงจะเปิด billing ได้ และ billing account อยู่ในสถานะ closed ทำให้สร้าง VM ไม่ได้
- AWS Free Plan (บัญชีที่สมัครหลัง 15 ก.ค. 2025) ให้ credit $100 ไม่ต้องจ่ายล่วงหน้า และไม่เก็บเงินจริงจนกว่าจะกด upgrade เอง อายุ 6 เดือน (ถึง 19 พ.ค. 2027) หรือจนกว่า credit หมด
- Free Plan **จำกัดให้ใช้ได้ region เดียว** บัญชีนี้ถูกผูกกับ `ap-southeast-2` (Sydney) ถ้าจะใช้ Oregon ต้อง upgrade เป็น paid

## ตัดสินใจ

- VM: **EC2 t3.micro** (2 vCPU, 1 GB RAM) · Ubuntu 24.04 x86_64 · gp3 20 GB · swap 2 GB · Elastic IP · CPU credit แบบ **Standard** (ไม่ใช่ Unlimited) เพื่อไม่ให้มีค่าใช้จ่ายเกินคาด
- Region: **ap-southeast-2 (Sydney)** และย้าย Neon กับ Atlas M0 ไป Sydney ด้วย ให้ API กับ DB อยู่ region เดียวกัน
- โดเมน: DuckDNS (ฟรี) ชี้ไปที่ Elastic IP · TLS จาก Let's Encrypt (certbot)
- CD: GitHub Actions build image → GHCR → SSH เข้า VM ด้วย deploy key แยกต่างหาก → `deploy/scripts/deploy.sh` (migration → `compose up` → เช็ก `/health/ready` → rollback อัตโนมัติถ้าไม่ผ่าน)
- Secret ของ DB อยู่ใน `/opt/stockscribe/.env` บน VM เท่านั้น ไม่อยู่ใน GitHub Secrets

## ผลที่ตามมา

- ✅ ไม่ต้องจ่ายเงินจริงจนถึง พ.ค. 2027 · ค่าใช้จ่ายโดยประมาณ ~$11/เดือน (t3.micro + public IPv4 + EBS) หักจาก credit
- ✅ Sydney อยู่ใกล้ไทยกว่า Oregon และ API คุยกับ DB ภายใน region เดียวกัน
- ⚠️ Free Plan หมดอายุ 19 พ.ค. 2027 · ก่อนวันนั้นต้องเลือกว่าจะ upgrade, ย้ายไป Azure for Students (มีอีเมลมหาวิทยาลัย) หรือปิดเดโม
- ⚠️ GitHub Actions ต้อง SSH เข้า VM ได้ จึงเปิดพอร์ต 22 ให้ทุก IP โดยรับเฉพาะ key (ไม่มีรหัสผ่าน) และตรวจ host key ผ่าน `EC2_KNOWN_HOSTS`
- ⚠️ Migration ไม่ถูก rollback ตาม image ต้องเขียนแบบ backward compatible (เพิ่มก่อน ลบใน release ถัดไป)

## ทางเลือกที่ไม่เลือก

- **GCE e2-micro:** ฟรีตลอด แต่ต้อง prepay และ billing account ใช้ไม่ได้
- **Upgrade AWS เป็น paid เพื่อใช้ Oregon:** เสี่ยงโดนเก็บเงินจริงจากบัตร และ Sydney ไม่ได้แย่กว่าสำหรับผู้ใช้ในไทย
- **VM ที่ Sydney แต่ DB ที่ Oregon:** ไม่ต้องย้าย DB แต่ทุก query ต้องข้ามมหาสมุทร (~140 ms ต่อครั้ง)
- **Lightsail:** ง่ายกว่า แต่ควบคุม security group และ IAM ได้น้อยกว่า
- **SSM Session Manager + OIDC แทน SSH:** ไม่ต้องเปิดพอร์ต 22 และไม่มี key ค้างใน GitHub ปลอดภัยกว่า แต่ต้องตั้ง IAM role และ instance profile เพิ่ม · เก็บไว้ทำทีหลัง
- **Caddy แทน Nginx + certbot:** ได้ HTTPS อัตโนมัติด้วย config สั้นกว่า แต่ขัดกับ ADR-0004 ที่ใช้ Nginx อยู่แล้ว
