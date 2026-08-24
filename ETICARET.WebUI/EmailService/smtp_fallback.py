import json
import smtplib
import ssl
import sys
from email.message import EmailMessage
from email.utils import formataddr


def main():
    settings = json.load(sys.stdin)
    message = EmailMessage()
    message["From"] = formataddr((settings["fromName"], settings["fromAddress"]))
    message["To"] = settings["recipient"]
    message["Subject"] = settings["subject"]

    if settings["isHtml"]:
        message.set_content("Bu e-postayı görüntülemek için HTML destekli bir e-posta uygulaması kullanın.")
        message.add_alternative(settings["body"], subtype="html")
    else:
        message.set_content(settings["body"])

    context = ssl.create_default_context()
    if settings["enableSsl"] and settings["port"] == 465:
        client = smtplib.SMTP_SSL(settings["host"], settings["port"], timeout=20, context=context)
    else:
        client = smtplib.SMTP(settings["host"], settings["port"], timeout=20)
        if settings["enableSsl"]:
            client.starttls(context=context)

    try:
        client.login(settings["username"], settings["password"])
        client.send_message(message)
    finally:
        client.quit()

    print("OK")


if __name__ == "__main__":
    main()
