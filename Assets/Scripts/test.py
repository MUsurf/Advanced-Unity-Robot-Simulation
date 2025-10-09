import rospy
from std_msgs.msg import String

rospy.init_node('file_content_publisher')
pub = rospy.Publisher('/imu', String, queue_size=10)

while not rospy.is_shutdown():
    try:
        with open("robotdata.txt", "r") as file:
            content = file.read()
            pub.publish(content)
    except Exception as e:
        print("ohno:", e)
    rospy.sleep(1)