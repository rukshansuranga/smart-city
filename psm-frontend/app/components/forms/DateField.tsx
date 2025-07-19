import { Label, HelperText, Datepicker } from "flowbite-react";
import { UseControllerProps, useController } from "react-hook-form";

type Props = {
  label?: string;
  placeholder?: string;
  value?: Date;
  showlabel?: boolean;
  className?: string;
  required?: boolean;
} & UseControllerProps;

const DateField: React.FC<Props> = (props: Props) => {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <div className="w-full">
      {props?.showlabel && (
        <div className="mt-2 mb-1 block">
          <Label htmlFor={props.name}>{props.label}</Label>
          {props.required && <span className="text-red-600">*</span>}
        </div>
      )}
      {/* <TextInput
        {...props}
        {...field}
        type={props.type || "text"}
        placeholder={props.placeholder}
        className={props.className}
        color={
          fieldState.error ? "failure" : !fieldState.isDirty ? "" : "success"
        }
      /> */}
      <Datepicker {...props} {...field} />
      <HelperText>{fieldState.error?.message}</HelperText>
    </div>
  );
};
export default DateField;
